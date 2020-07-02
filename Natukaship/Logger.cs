using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Natukaship
{
    public static class Logger
    {
        public static string Filename { get => $"natukaship_{DateTime.Now.ToString("yyyy-MM-dd")}.log"; }
        public static string FileDirectory { get => $"{Directory.GetCurrentDirectory()}/natukaship/logs/"; }

        public static void Warn(string message)
        {
            if (!File.Exists(FileDirectory + Filename))
                CreateLogFile();

            string log = $"WARN  [{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff")}]: {message}\n";
            File.AppendAllText(FileDirectory + Filename, log);
        }

        public static void Info(string message)
        {
            if (!File.Exists(FileDirectory + Filename))
                CreateLogFile();

            string log = $"INFO  [{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff")}]: {message}\n";
            File.AppendAllText(FileDirectory + Filename, log);
        }

        public static void Debug(string message)
        {
            if (!File.Exists(FileDirectory + Filename))
                CreateLogFile();

            string log = $"DEBUG [{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff")}]: {message}\n";
            File.AppendAllText(FileDirectory + Filename, log);
        }

        static void CreateLogFile()
        {
            try
            {
                File.Create(FileDirectory + Filename).Dispose();
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(FileDirectory);
                File.Create(FileDirectory + Filename).Dispose();
            }
            File.AppendAllText(FileDirectory + Filename, $"File Created at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff")}\n");
        }
    }
}
