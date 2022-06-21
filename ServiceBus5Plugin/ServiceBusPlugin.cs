using System.Diagnostics;
using Azure.Messaging.ServiceBus;
using PluginBase;

namespace ServiceBusPlugin
{
    public class ServiceBusPlugin : ICommand
    {
        private ServiceBusSender _sender;
        public string Name => "ServiceBusPlugin";

        public string Description => "Service Bus Plugin";

        public void Execute()
        {
            Console.WriteLine("Execute -------");
            ShowAssemblyNameAndVersion(typeof(ServiceBusPlugin));
            ShowAssemblyNameAndVersion(_sender.GetType());
        }

        public void Initialize(IDictionary<string, string> context)
        {
            Console.WriteLine("Initialize ServiceBus Plugin -----");
            Console.WriteLine($"ConnectionString: {context["connectionString"]} queueName: {context["queueName"]}");
            var client = new ServiceBusClient(context["connectionString"]);
            _sender = client.CreateSender(context["queueName"]);
        }

        private void ShowAssemblyNameAndVersion(Type t)
        {
            var currentAssembly = t.Assembly;
            var fileVersion = FileVersionInfo.GetVersionInfo(currentAssembly.Location);
            Console.WriteLine($"Assembly Name : {currentAssembly.FullName} Version : {fileVersion.FileVersion}");
        }
    }
}