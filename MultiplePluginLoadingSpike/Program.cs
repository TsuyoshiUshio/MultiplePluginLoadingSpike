using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.Logging;
using MultiplePluginLoadingSpike.PluginLoader;
using PluginBase;

namespace MultiplePluginLoadingSpike
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 1 && args[0] == "/d")
                {
                    Console.WriteLine("Waiting for any key...");
                    Console.ReadLine();
                }

                // Load commands from plugins.
                string[] pluginPath = new string[]
                {
                    "ServiceBus4Plugin\\bin\\Debug\\net6.0\\ServiceBusPlugin.dll",
                    "ServiceBus5Plugin\\bin\\Debug\\net6.0\\ServiceBusPlugin.dll",
                };
                IEnumerable<ICommand> commands = pluginPath.SelectMany(pluginPath =>
                {
                    Assembly pluginAssembly = LoadPlugin(pluginPath);

                    return CreateCommands(pluginAssembly);
                }).ToList();

                IDictionary<string, string> context = new Dictionary<string, string>();
                context.Add("connectionString", Environment.GetEnvironmentVariable("ServiceBusConnectionString"));
                context.Add("queueName", Environment.GetEnvironmentVariable("ServiceBusQueueName"));

                var loggerFactory = new LoggerFactory()
                    .AddConsole();

                foreach (ICommand command in commands)
                {
                    command.Initialize(context, loggerFactory);
                }

                foreach (ICommand command in commands)
                {
                    command.Execute();
                }

                // The following reflection fails
                // var instance = Activator.CreateInstance("ServiceBusPlugin", "ServiceBusPlugin.ServiceBusPlugin");
                // Console.WriteLine($"V5 : {instance.GetType().FullName} {instance.GetType().Assembly.Location}");
                var defaultAssembly = typeof(Program).Assembly;
                Console.WriteLine("*************************** List types");
                foreach (var t in defaultAssembly.GetTypes())
                {
                    Console.WriteLine(t.FullName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static Assembly LoadPlugin(string relativePath)
        {
            // Navigate up to the solution root
            string root = Path.GetFullPath(Path.Combine(
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(
                                Path.GetDirectoryName(typeof(Program).Assembly.Location)))))));
            string pluginLocation = Path.GetFullPath(Path.Combine(root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
            Console.WriteLine($"Loading commands from: {pluginLocation}");

            //   PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            AssemblyLoadContext loadContext = CreateScaleControllerAssemblyLoadContext(pluginLocation);
            loadContext.Resolving += CustomEventHandler;
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += CustomEventHandlerForAppDomain;
            AssemblyName assemblyName = new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation));
            return loadContext.LoadFromAssemblyName(assemblyName);
        }

        private static AssemblyLoadContext CreatePluginAssemblyLoadContext(string pluginLocation)
        {
            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            loadContext.Resolving += CustomEventHandler;
            return loadContext;
        }

        private static AssemblyLoadContext CreateScaleControllerAssemblyLoadContext(string pluginLocation)
        {
            var path = Path.GetDirectoryName(pluginLocation);
            return new ScaleControllerAssemblyLoadContext(path);
        }

        private static Assembly CustomEventHandlerForAppDomain(object sender, ResolveEventArgs args)
        {
            Console.WriteLine("Resolving...");
            return typeof(Program).Assembly;
        }

        private static Assembly? CustomEventHandler(AssemblyLoadContext context, AssemblyName assemblyName)
        {
            Console.WriteLine($"Coundn't resovle {assemblyName.Name}");
            return null; // Fail to read. 
        }

        static IEnumerable<ICommand> CreateCommands(Assembly assembly)
        {
            int count = 0;

            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(ICommand).IsAssignableFrom(type))
                {
                    ICommand? result = Activator.CreateInstance(type) as ICommand;
                    if (result != null)
                    {
                        count++;
                        yield return result;
                    }
                }
            }

            if (count == 0)
            {
                string availableTypes = string.Join(",", assembly.GetTypes().Select(t => t.FullName));
                throw new ApplicationException(
                    $"Can't find any type which implement ICommand in {assembly} from {assembly.Location}.\n" +
                    $"Available types: {availableTypes}");
            }
        }
    }
}
