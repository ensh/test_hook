using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace FileMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            Init();

            Run(args);
        }

        private static void Init()
        {
            Console.WriteLine(Logger.Log("Загрузка плагинов обработки"));

            foreach (var pluginsInfo in PluginContainer.Plugins)
            {
                Console.WriteLine(Logger.Log(Environment.NewLine + "Расширение файла: " + pluginsInfo.Key));
                foreach (var pluginInfo in pluginsInfo.Value)
                {
                    Console.WriteLine(Logger.Log(pluginInfo.Description));
                }
            }

            Console.WriteLine(Logger.Log("--------------------------------------------"));
        }

        private static void Run(string[] args)
        {
            using (FileSystemWatcher fileSystemWatcher = new FileSystemWatcher())
            {
                fileSystemWatcher.Path = (args.Length > 1) ? args[1] : Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                fileSystemWatcher.Created += (s, e) => OnFileCreated(e);

                fileSystemWatcher.EnableRaisingEvents = true;

                Console.ReadLine();
            }
        }

        private static void OnFileCreated(FileSystemEventArgs e)
        {
            if (!File.GetAttributes(e.FullPath).HasFlag(FileAttributes.Directory))
            {
                Thread.Sleep(2000); // без sleep иногда схватывает эксепшен доступа к файлу
                using (var stream = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        foreach (var result in PluginContainer.ApplyPlugins(Path.GetExtension(e.Name), stream, null))
                        {
                            var resultText = string.Concat(e.Name, "-", result.Key, "-", result.Value.ToString());
                            Console.WriteLine(Logger.Log(resultText));
                        }
                    }
                    catch (ProcessingException pe)
                    {
                        var resultText = string.Concat(e.Name, "-", pe.Message);
                        Console.WriteLine(Logger.Log(resultText));
                    }
                }
            }
        }        
    }
}
