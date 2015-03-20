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

        public static void Write(String msg, String fileName)
        {
            try
            {
                // Путь .\\Log
                string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                if (!Directory.Exists(pathToLog))
                    Directory.CreateDirectory(pathToLog); // Создаем директорию, если нужно
                string filename = Path.Combine(pathToLog, string.Format("{0}_{1:dd.MM.yyy}.csv",
                AppDomain.CurrentDomain.FriendlyName, DateTime.Now));
                string fullText = string.Format(msg);
                lock (sync)
                {
                    File.AppendAllText(filename, fullText+ ";" + Environment.NewLine, Encoding.GetEncoding("Windows-1251"));
                }
            }
            catch
            {
                // Перехватываем все и ничего не делаем
            }
        }
    }
}
