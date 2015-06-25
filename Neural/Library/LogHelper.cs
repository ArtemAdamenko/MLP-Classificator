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
        //private static object sync = new object();
        private static String time;
        private static int netsCounter = 0;
        private static String path = AppDomain.CurrentDomain.BaseDirectory;

        #region CONSTS
        const string RESULT_CORRECT_FILE = "РезультатыКорректировки-";
        const string VARIATIONS_FILE = "Коэф.вариации-";
        const string MEDIUMS_FILE = "СредниеЗначения-";
        const string ROOT_FOLDER = "Evolution";
        const string SAVE_CONNECT_DIR = "Конф. соединениния подсети и основной ИНС";
        const string SELECTION_DIR = "Отбор-";
        const string CROSS_DIR = "Скрещивание-";
        const string ALLWEIGHTS_FILE = "ВесаВсехСетей-";
        const string DEVIATION_FILE = "СтандартноеОтклонение-";
        const string PARAMS_CORRECT_FILE = "ПараметрыКоррекции-";
        #endregion

        #region WorkBook, WorkSheets
        private static ExcelPackage results;
        private static ExcelWorksheet resultsSheet;
        private static ExcelPackage coefvariations;
        private static ExcelWorksheet coefvariationsSheet;
        private static ExcelPackage mediumsBook;
        private static ExcelWorksheet mediumsSheet;
        private static ExcelPackage allWeightsBook;
        private static ExcelWorksheet allWeightsSheet;
        private static ExcelPackage deviationBook;
        private static ExcelWorksheet deviationSheet;
        private static ExcelPackage correctParamsBook;
        private static ExcelWorksheet correctParamsSheet;
        #endregion

        #region files, folders
        private static String currentFolder = "";
        private static FileInfo resultsFile;
        private static FileInfo covarFile;
        private static FileInfo deviationFile;
        private static FileInfo mediumsFile;
        private static FileInfo allWeightsFile;
        private static FileInfo correctParamsFile;
        #endregion

        public static String getTime()
        {   
            return time = DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss");
        }

        //создание новой папки для новой сессии
        public static void NewSessionFolder(String commonNetFileName, String dataFileName)
        {
            LogHelper.currentFolder = ROOT_FOLDER + "\\" + LogHelper.getTime() + " (" + commonNetFileName + " - " + dataFileName + ")";
            return;
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
        public static void saveMSGToSheet(List<String> content, int population)
        {

            int i = 0;
            foreach (string str in content)
            {
                LogHelper.resultsSheet.Cells[i + 1, population + 1].Value = str;
                i++;
            }

            return;
        }

        //запись конфигурации подсоединение подсети к основной ИНС в файлы
        public static void SubConnectReport(List<String> inputConnected, List<String> outputConnected, int connectingCount)
        {
            //check exist folder for connects config
            string folder = LogHelper.currentFolder + "\\" + SAVE_CONNECT_DIR;
            LogHelper.checkDir(folder);

            FileInfo fileInputs = new FileInfo(folder + "\\ТочкиДляВходов-" + connectingCount + ".xlsx");
            ExcelPackage bookInputs = new ExcelPackage(fileInputs);
            ExcelWorksheet sheetInputs = bookInputs.Workbook.Worksheets.Add("ТочкиДляВходов");

            FileInfo fileOutputs = new FileInfo(folder + "\\ТочкиДляВыходов-" + connectingCount + ".xlsx");
            ExcelPackage bookOutputs = new ExcelPackage(fileOutputs);
            ExcelWorksheet sheetOutputs = bookOutputs.Workbook.Worksheets.Add("ТочкиДляВыходов");

            //headers
            sheetInputs.Cells[1, 1].Value = "Индекс вх. нейрона подсети";
            sheetOutputs.Cells[1, 1].Value = "Индекс вых. нейрона подсети";
            sheetOutputs.Cells[1, 2].Value = sheetInputs.Cells[1, 2].Value = "Слой ИНС";
            sheetOutputs.Cells[1, 3].Value = sheetInputs.Cells[1, 3].Value = "Нейрон ИНС";
            sheetOutputs.Cells[1, 4].Value = sheetInputs.Cells[1, 4].Value = "Вес ИНС";

            //save inputs
            int i = 0;
            foreach (String connect in inputConnected)
            {
                String[] temp = connect.Split(':');

                sheetInputs.Cells[i + 2, 1].Value = i;
                sheetInputs.Cells[i + 2, 2].Value = Int32.Parse(temp[0]);
                sheetInputs.Cells[i + 2, 3].Value = Int32.Parse(temp[1]);
                sheetInputs.Cells[i + 2, 4].Value = Int32.Parse(temp[2]);
                i++;
            }

            //save outputs
            i = 0;
            foreach (String connect in outputConnected)
            {
                String[] temp = connect.Split(':');

                sheetOutputs.Cells[i + 2, 1].Value = i;
                sheetOutputs.Cells[i + 2, 2].Value = Int32.Parse(temp[0]);
                sheetOutputs.Cells[i + 2, 3].Value = Int32.Parse(temp[1]);
                sheetOutputs.Cells[i + 2, 4].Value = Int32.Parse(temp[2]);
                i++;
            }

            bookInputs.Save();
            bookOutputs.Save();

            return;
        }

        //запись весов отобранных или скрещенных сетей
        public static void SubnetToFile(Subnet net, int reconnectingCount, Boolean cross, string parents = "", int population = 0)
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

                LogHelper.allWeightsSheet.Cells[ 1, netsCounter + 1].Value = net.ID + "-" + parents + "-" + net.quality;


                int row = 0;
                for (int i = 0; i < net.Network.Layers.Length; i++)
                {
                    for (int j = 0; j < net.Network.Layers[i].Neurons.Length; j++)
                    {
                        for (int k = 0; k < net.Network.Layers[i].Neurons[j].Weights.Length; k++)
                        {

                            file.WriteLine(i.ToString() + ";" + j.ToString() + ";" + k.ToString() + ";" + net.Network.Layers[i].Neurons[j].Weights[k] + ";");
                            
                            LogHelper.allWeightsSheet.Cells[row + 2, netsCounter + 1].Value = net.Network.Layers[i].Neurons[j].Weights[k];
                            row++;
                        }
                    }
                }
                file.Flush();
                file.Close();
                netsCounter++;
            }
            catch
            {
                // Перехватываем все и ничего не делаем
            }
        }

        //коэф.вариации по каждому индексу весов подсетей
        public static void coefVariationsToFile(double[] coefficients, int population,  string msg = "")
        {

            LogHelper.coefvariationsSheet.Cells[ 1, population + 1].Value = msg;
            for (int i = 0; i < coefficients.Length; i++)
            {
                LogHelper.coefvariationsSheet.Cells[i + 2, population + 1].Value = coefficients[i];
            }

            return;
        }

        //сохранение средних значений по каждому индексу весов подсетей
        public static void saveMediumsValuesOfWeightsIndexes(List<Subnet> subnets, int population, string msg = "")
        {
            mediumsSheet.Cells[ 1, population + 1].Value = msg;
            int row = 1;
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
            return;
        }

        //сохранение входной выборки с соответствующими выходными значениями корректировки
        public static void InputVectorResults( String filename, double[] desireClass, double[] outputClass )
        {
            LogHelper.checkDir(LogHelper.currentFolder);

            FileInfo file = new FileInfo(LogHelper.currentFolder + "\\" + filename + ".xlsx");
            ExcelPackage book = new ExcelPackage(file);
            ExcelWorksheet sheet = book.Workbook.Worksheets.Add(filename);


            if (desireClass.Length != outputClass.Length)
                throw new Exception();

            for (int i = 0; i < desireClass.Length; i++)
            {
                sheet.Cells[i + 1, 1].Value = desireClass[i];
                sheet.Cells[i + 1, 2].Value = outputClass[i];
            }
            book.Save();
            return;
        }

        //сохранение показателей корректировки по поколениям
        public static void commonCorrectResults(double medium, double better, double coefVariation, int population)
        {
            LogHelper.correctParamsSheet.Cells[population + 1, 1].Value = "Поколение " + population;
            LogHelper.correctParamsSheet.Cells[population + 1, 2].Value = medium;
            LogHelper.correctParamsSheet.Cells[population + 1, 3].Value = better;
            LogHelper.correctParamsSheet.Cells[population + 1, 4].Value = coefVariation;
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

            //all weights file
            LogHelper.allWeightsBook.Save();

            //standart deviation file
            LogHelper.deviationBook.Save();

            //correct params book
            LogHelper.correctParamsBook.Save();

            return;
        }

        //Инициализирует папки, файлы и spreadsheets 
        public static void InitReporting(Subnet subnet = null, int reconnectingCount = 0) 
        {

            LogHelper.checkDir(LogHelper.currentFolder);
            LogHelper.netsCounter = 0;

            //создание файла с результатами корректировки
            LogHelper.resultsFile = new FileInfo(LogHelper.currentFolder + "\\" + RESULT_CORRECT_FILE + reconnectingCount.ToString() + ".xlsx");
            LogHelper.results = new ExcelPackage(LogHelper.resultsFile);
            LogHelper.resultsSheet = LogHelper.results.Workbook.Worksheets.Add("Результаты");

            //создание файла с коэф.вариации
            LogHelper.covarFile = new FileInfo(LogHelper.currentFolder + "\\" + VARIATIONS_FILE + reconnectingCount.ToString() + ".xlsx");
            LogHelper.coefvariations = new ExcelPackage(LogHelper.covarFile);
            LogHelper.coefvariationsSheet = LogHelper.coefvariations.Workbook.Worksheets.Add("Коэф.вариации");

            //стандартное отклонение
            LogHelper.deviationFile = new FileInfo(LogHelper.currentFolder + "\\" + DEVIATION_FILE + reconnectingCount.ToString() + ".xlsx");
            LogHelper.deviationBook = new ExcelPackage(LogHelper.deviationFile);
            LogHelper.deviationSheet = LogHelper.deviationBook.Workbook.Worksheets.Add("Стандартные отклонения весов");

            //средние значения
            LogHelper.mediumsFile = new FileInfo(LogHelper.currentFolder + "\\" + MEDIUMS_FILE + reconnectingCount.ToString() + ".xlsx");
            LogHelper.mediumsBook = new ExcelPackage(LogHelper.mediumsFile);
            LogHelper.mediumsSheet = LogHelper.mediumsBook.Workbook.Worksheets.Add("Средние значения");

            //веса всех сетей
            LogHelper.allWeightsFile = new FileInfo(LogHelper.currentFolder + "\\" + ALLWEIGHTS_FILE + reconnectingCount.ToString() + ".xlsx");
            LogHelper.allWeightsBook = new ExcelPackage(LogHelper.allWeightsFile);
            LogHelper.allWeightsSheet = LogHelper.allWeightsBook.Workbook.Worksheets.Add("Веса всех сетей сессии");

            //параметры коррекции по поколениям
            LogHelper.correctParamsFile = new FileInfo(LogHelper.currentFolder + "\\" + PARAMS_CORRECT_FILE + reconnectingCount.ToString() + ".xlsx");
            LogHelper.correctParamsBook = new ExcelPackage(LogHelper.correctParamsFile);
            LogHelper.correctParamsSheet = LogHelper.correctParamsBook.Workbook.Worksheets.Add("Показатели коррекции по поколениям");
            LogHelper.correctParamsSheet = LogHelper.setCommonCorrectParamsSheetHeaders(correctParamsSheet);

            if (subnet != null)
                LogHelper.setIndexesToSheet(subnet);

            return;
        }

        public static void StandartDeviationToFile(double[] deviation, int population, string msg = "")
        {
            LogHelper.deviationSheet.Cells[1, population + 1].Value = msg;
            for (int i = 0; i < deviation.Length; i++)
            {
                LogHelper.deviationSheet.Cells[i + 2, population + 1].Value = deviation[i];
            }

            return;
        }

        private static void setIndexesToSheet(Subnet subnet)
        {
            int count = ANNUtils.getCountOfWeights(subnet.Network);

            for (int i = 1; i <= count; i++)
            {
                mediumsSheet.Cells[ i + 1, 1].Value = "Вес " + (i-1).ToString();
                coefvariationsSheet.Cells[i + 1, 1].Value = "Вес " + (i - 1).ToString();
                deviationSheet.Cells[i + 1, 1].Value = "Вес " + (i - 1).ToString();
            }
        }
        //установление заголовков для фала параметров корректирования
        public static ExcelWorksheet setCommonCorrectParamsSheetHeaders(ExcelWorksheet commonCorrectParamsSheet)
        {
            commonCorrectParamsSheet.Cells[1, 2].Value = "Среднее значение поколения";
            commonCorrectParamsSheet.Cells[1, 3].Value = "Лучшее значение поколения";
            commonCorrectParamsSheet.Cells[1, 4].Value = "Коэф.вариации значений поколения";

            return commonCorrectParamsSheet;

        }
        //установление заголовков для отчета по отключения нейронов в xlsx файл
        public static ExcelWorksheet setNeuronsWorkBookHeaders(ExcelWorksheet QualityWorkSheet, Record[] rangeNeurons)
        {
            for (int header = 0; header < rangeNeurons.Length; header++)
            {
                QualityWorkSheet.Cells[1, header + 2].Value = rangeNeurons[header].numberLayer.ToString() + ":" + rangeNeurons[header].numberNeuron.ToString();
            }
            QualityWorkSheet.Cells[1, rangeNeurons.Length + 2].Value = "Число отключенных нейронов";
            QualityWorkSheet.Cells[1, rangeNeurons.Length + 3].Value = "ЧОН/Нейронов всего";
            QualityWorkSheet.Cells[1, rangeNeurons.Length + 4].Value = "Соотношение нейронов";

            return QualityWorkSheet;
        }

        //установление заголовков для отчета по связям нейронов в xls файл
        public static ExcelWorksheet setWeightsWorkBookHeaders(ExcelWorksheet QualityWorkSheet, Record[] rangeNeurons)
        {
            int step = 1;
            for (int header = 0; header < rangeNeurons.Length; header++)
            {

                QualityWorkSheet.Cells[1, step + 1].Value = rangeNeurons[header].numberLayer.ToString() + ":" + rangeNeurons[header].numberNeuron.ToString() + "|Вх.";
                QualityWorkSheet.Cells[1, step + 2].Value = rangeNeurons[header].numberLayer.ToString() + ":" + rangeNeurons[header].numberNeuron.ToString() + "|Исх.";
                step += 2;
            }
            return QualityWorkSheet;

        }
    }
}
