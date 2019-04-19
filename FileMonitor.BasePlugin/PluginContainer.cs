using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FileMonitor
{
    public static class PluginContainer
    {
        public const string PluginFileMask = "FileMonitor.Plugin*.dll";
        public static IEnumerable<KeyValuePair<string, object>> ApplyPlugins(string extension, FileStream dataStream, object context)
        {
            if (Plugins.TryGetValue(extension, out var plugins) || Plugins.TryGetValue("", out plugins))
            {
                foreach (IPlugin plugin in plugins)
                {
                    var processingResult = plugin.Process(dataStream, context);

                    if (processingResult != null)
                        yield return new KeyValuePair<string, object>(plugin.Description, processingResult);
                }
            }
        }

        public static IDictionary<string, IPlugin[]> Plugins
        {
            get
            {
                if (pluginContainer == null)
                {
                    pluginContainer = LoadPlugins(Directory.EnumerateFiles(".", PluginFileMask))
                        .ToArray()
                        .GroupBy(plugin => plugin.Extension ?? "")
                        .ToDictionary(p => p.Key, pp => pp.ToArray());
                }

                return pluginContainer;
            }
        }

        private static IDictionary<string, IPlugin[]> pluginContainer;

        private static IEnumerable<IPlugin> LoadPlugins(IEnumerable<string> fileNames)
        {
            foreach (var assemblyFileName in fileNames)
            {
                foreach (var type in GetTypes(assemblyFileName, typeof(IPlugin)))
                {
                    yield return (IPlugin)Activator.CreateInstance(type);
                }
            }
        }

        private static IEnumerable<Type> GetTypes(string assemblyFileName, Type interfaceFilter)
        {
            Assembly asm = Assembly.LoadFrom(assemblyFileName);
            foreach (Type type in asm.GetTypes())
            {
                if (type.GetInterface(interfaceFilter.Name) != null)
                {
                    yield return type;
                }
            }
        }
    }
}
