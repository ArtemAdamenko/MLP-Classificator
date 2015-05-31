using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using OfficeOpenXml;

namespace Neural
{
    class LogHelper
    {
        private static object sync = new object();
        private static String time;
        private static String path = AppDomain.CurrentDomain.BaseDirectory;

        #region CONSTS
        const string RESULT_CORRECT_FILE = "РезультатыКорректировки-";
        const string VARIATIONS_FILE = "Коэф.вариации-";
        const string MEDIUMS_FILE = "СредниеЗначения-";
        const string ROOT_FOLDER = "Evolution";
        const string SAVE_CONNECT_DIR = "Конф. соединениния подсети и основной ИНС";
        const string SELECTION_DIR = "Отбор-";
        const string CROSS_DIR = "Скрещивание-";
        #endregion

        #region WorkBook, WorkSheets
        private static ExcelPackage results;
        private static ExcelWorksheet resultsSheet;
        private static ExcelPackage coefvariations;
        private static ExcelWorksheet coefvariationsSheet;
        private static ExcelPackage mediumsBook;
        private static ExcelWorksheet mediumsSheet;
        #endregion

        #region files, folders
        private static String currentFolder = "";
        private static FileInfo resultsFile;
        private static FileInfo covarFile;
        private static FileInfo mediumsFile;
        #endregion

        public static String getTime()
        {   
            return time = DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss");
        }

        //создание новой папки для новой сессии
        public static void NewSessionFolder()
        {
            LogHelper.currentFolder = ROOT_FOLDER + "\\" + LogHelper.getTime();



        }
        public static String getPath(String folderName)
        {
            string pathToLog = "";
            try
            {
                pathToLog = Path.Combine(path, folderName);
                if (!Directory.Exists(pathToLog))
                    Directory.CreateDirectory(pathToLog);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return pathToLog;
        }

        //проверка и создание папки для лога
        private static void checkDir(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        //запись результатов после отбора или скрещивания, пачками
        public static void saveMSGToSheet(List<String> content, int population, int reconnectingCount)
        {

            LogHelper.checkDir( LogHelper.currentFolder );

            LogHelper.resultsFile = new FileInfo( LogHelper.currentFolder + "\\" + RESULT_CORRECT_FILE + reconnectingCount.ToString() + ".xlsx" );

            LogHelper.results = new ExcelPackage(LogHelper.resultsFile);
            LogHelper.resultsSheet = LogHelper.results.Workbook.Worksheets.Add("Результаты");

            int i = 0;
            foreach (string str in content)
            {
                LogHelper.resultsSheet.Cells[i + 1, population + 1].Value = content[i];
                i++;
            }
          //  LogHelper.results.Save();

            return;
        }

        //запись конфигурации подсоединение подсети к основной ИНС в файлы
        public static void SubConnectReport(List<String> inputConnected, List<String> outputConnected, int connectingCount)
        {
            //check exist folder for connects config
            string folder = LogHelper.currentFolder + "\\" + SAVE_CONNECT_DIR;
            LogHelper.checkDir(folder);

            //create two files - for inputs and outputs connects config save 
            System.IO.StreamWriter fileInputs = new System.IO.StreamWriter(folder + "\\ТочкиДляВходов-" + connectingCount + ".csv");
            System.IO.StreamWriter fileOutputs = new System.IO.StreamWriter(folder + "\\ТочкиДляВыходов-" + connectingCount + ".csv");

            fileInputs.WriteLine("Индекс вх. нейрона подсети;Слой ИНС;Нейрон ИНС;Вес ИНС;");
            fileOutputs.WriteLine("Индекс вх. нейрона подсети;Слой ИНС;Нейрон ИНС;Вес ИНС;");

            //save inputs
            int i = 0;
            foreach (String connect in inputConnected)
            {
                fileInputs.WriteLine(i.ToString() + ";" + connect.Replace(':', ';'));
                i++;
            }
            fileInputs.Flush();

            //save outputs
            i = 0;
            foreach (String connect in outputConnected)
            {
                fileOutputs.WriteLine(i.ToString() + ";" + connect.Replace(':', ';'));
                i++;
            }
            fileOutputs.Flush();

            return;
        }

        //запись весов отобранных или скрещенных сетей
        public static void WriteEvo(Subnet net, int reconnectingCount, Boolean cross, string parents = "", int population = 0)
        {
            try
            {
                string folder = "";
                if (cross)
                    folder = currentFolder + "\\" + CROSS_DIR + reconnectingCount.ToString() + "\\" + population + "\\";
                else
                    folder = currentFolder + "\\" + SELECTION_DIR + reconnectingCount.ToString() + "\\";

                LogHelper.checkDir(folder);

                string filename = Path.Combine(folder, net.ID + "-" + parents + "-" + net.quality + ".csv");

                System.IO.StreamWriter file = new System.IO.StreamWriter(filename);

                file.WriteLine("Layer;Neuron;Weight;Value;");

                for (int i = 0; i < net.Network.Layers.Length; i++)
                {
                    for (int j = 0; j < net.Network.Layers[i].Neurons.Length; j++)
                    {
                        for (int k = 0; k < net.Network.Layers[i].Neurons[j].Weights.Length; k++)
                        {

                            file.WriteLine(i.ToString() + ";" + j.ToString() + ";" + k.ToString() + ";" + net.Network.Layers[i].Neurons[j].Weights[k] + ";");

                        }
                    }
                }
                file.Flush();
            }
            catch
            {
                // Перехватываем все и ничего не делаем
            }
        }

        //коэф.вариации по каждому индексу весов подсетей
        public static void coefVariationsToFile(double[] coefficients, int population, int reconnectingCount, string msg = "")
        {
            //string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, currentFolder);

            LogHelper.checkDir( LogHelper.currentFolder );

            LogHelper.covarFile =  new FileInfo( LogHelper.currentFolder + "\\" + VARIATIONS_FILE + reconnectingCount.ToString() + ".xlsx" );
            LogHelper.coefvariations = new ExcelPackage(LogHelper.covarFile);
            LogHelper.coefvariationsSheet = LogHelper.coefvariations.Workbook.Worksheets.Add("Коэф.вариации");


            LogHelper.coefvariationsSheet.Cells[ 1, population + 1].Value = msg;
            for (int i = 0; i < coefficients.Length; i++)
            {
                LogHelper.coefvariationsSheet.Cells[i + 2, population + 1].Value = coefficients[i].ToString();
            }

            LogHelper.coefvariations.Save();
            return;
        }

        //сохранение средних значений по каждому индексу весов подсетей
        public static void saveMediumsValuesOfWeightsIndexes( List<Subnet> subnets, int population, int reconnectingCount )
        {
            LogHelper.checkDir( LogHelper.currentFolder );

            LogHelper.mediumsFile = new FileInfo( LogHelper.currentFolder + "\\" + MEDIUMS_FILE + reconnectingCount.ToString() + ".xlsx" );
            LogHelper.mediumsBook = new ExcelPackage(LogHelper.mediumsFile);
            LogHelper.mediumsSheet = LogHelper.mediumsBook.Workbook.Worksheets.Add("Средние значения");
            int row = 0;
            for (int layer = 0; layer < subnets[0].Network.Layers.Length; layer++)
            {
                for (int neuron = 0; neuron < subnets[0].Network.Layers[layer].Neurons.Length; neuron++)
                {
                    for (int weight = 0; weight < subnets[0].Network.Layers[layer].Neurons[neuron].Weights.Length; weight++)
                    {
                        double res = 0;
                        double summ = 0;
                        for (int net = 0; net < subnets.Count; net++)
                        {
                            summ += subnets[net].Network.Layers[layer].Neurons[neuron].Weights[weight];
                        }
                        res = summ / subnets.Count;
                        mediumsSheet.Cells[row + 1, population + 1].Value = res;
                        row++;
                    }
                }
            }
            LogHelper.mediumsBook.Save();
            return;
        }

        //сохранение входной выборки с соответствующими выходными значениями корректировки
        public static void InputVectorResults( String filename, double[] desireClass, double[] outputClass )
        {
            LogHelper.checkDir(LogHelper.currentFolder);
            string file = Path.Combine(LogHelper.currentFolder, filename + ".csv");
            

            System.IO.StreamWriter fileInputs = new System.IO.StreamWriter(file);

            if (desireClass.Length != outputClass.Length)
                throw new Exception();

            for (int i = 0; i < desireClass.Length; i++)
            {
                fileInputs.WriteLine(desireClass[i].ToString() + ";" + outputClass[i].ToString());
            }
            fileInputs.Flush();
            return;
        }

        public static void Commit()
        {
            //commit Results File
            LogHelper.results.Save();


            //commit coef variations file
            LogHelper.coefvariations.Save();


            //commit mediums file
            LogHelper.mediumsBook.Save();

            return;
        }
    }
}
