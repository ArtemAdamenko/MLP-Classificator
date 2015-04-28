using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Neural
{
    class LogHelper
    {
        private static object sync = new object();
        private static String time;
        private static String path = AppDomain.CurrentDomain.BaseDirectory;

        public static String getTime()
        {   
            return time = DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss");
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

        public static void WriteEvo(string path, Subnet net, string parents = "")
        {
            try
            {
                // Путь .\\Log
                string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path.ToString());
                if (!Directory.Exists(pathToLog))
                    Directory.CreateDirectory(pathToLog); // Создаем директорию, если нужно
                string filename = Path.Combine(pathToLog, net.ID + "_" + parents + "_" + net.quality + ".csv");

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
                    for (int i = 0; i < net.Network.Layers.Length; i++)
                    {
                        for (int j = 0; j < net.Network.Layers[i].Neurons.Length; j++)
                        {
                            for (int k = 0; k < net.Network.Layers[i].Neurons[j].Weights.Length; k++)
                            {

                                file.WriteLine("L[" + i.ToString() + "]N[" + j.ToString() + "]W[" + k.ToString() + "];" + net.Network.Layers[i].Neurons[j].Weights[k] + ";");

                            }
                        }
                    }
            }
            catch
            {
                // Перехватываем все и ничего не делаем
            }
        }

    }
}
