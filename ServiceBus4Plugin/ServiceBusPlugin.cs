using System.Diagnostics;
using Microsoft.Azure.ServiceBus;
using PluginBase;

namespace ServiceBusPlugin
{
    public class ServiceBusPlugin : ICommand
    {
        private QueueClient? _queueClient;
        public string Name => "ServiceBusPlugin";

        public string Description => "Service Bus Plugin";

        public void Execute()
        {
            Console.WriteLine("Execute -------");
            ShowAssemblyNameAndVersion(typeof(ServiceBusPlugin));
            ShowAssemblyNameAndVersion(_queueClient.GetType());
           
        }

        public void Initialize(IDictionary<string, string> context)
        {
            Console.WriteLine("Initialize ServiceBus Plugin -----");
            Console.WriteLine($"ConnectionString: {context["connectionString"]} queueName: {context["queueName"]}");
            _queueClient = new QueueClient(context["connectionString"], context["queueName"]);
        }

        private void ShowAssemblyNameAndVersion(Type t)
        {
            var currentAssembly = t.Assembly;
            var fileVersion = FileVersionInfo.GetVersionInfo(currentAssembly.Location);
            Console.WriteLine($"Assembly Name : {currentAssembly.FullName} Version : {fileVersion.FileVersion}");
        }


    }
}