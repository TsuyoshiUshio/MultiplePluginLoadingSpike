using System.Diagnostics;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using PluginBase;

namespace ServiceBusPlugin
{
    public class ServiceBusPlugin : ICommand
    {
        private ServiceBusSender _sender;
        private ILogger _logger;
        public string Name => "ServiceBusPlugin";

        public string Description => "Service Bus Plugin";

        public void Execute()
        {
            _logger.LogInformation("Execute -------");
            ShowAssemblyNameAndVersion(typeof(ServiceBusPlugin));
            ShowAssemblyNameAndVersion(_sender.GetType());
        }

        public void Initialize(IDictionary<string, string> context, ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger<ServiceBusPlugin>();
            _logger.LogInformation("Initialize ServiceBus Plugin -----");
            _logger.LogInformation($"ConnectionString: {context["connectionString"]} queueName: {context["queueName"]}");
            var client = new ServiceBusClient(context["connectionString"]);
            _sender = client.CreateSender(context["queueName"]);
        }

        private void ShowAssemblyNameAndVersion(Type t)
        {
            var currentAssembly = t.Assembly;
            var fileVersion = FileVersionInfo.GetVersionInfo(currentAssembly.Location);
            _logger.LogInformation($"Assembly Name : {currentAssembly.FullName} Version : {fileVersion.FileVersion}");

            var instance = Activator.CreateInstance("ServiceBusPlugin", "ServiceBusPlugin.ServiceBusPlugin");
            _logger.LogInformation($"V5 : {instance.GetType().FullName} {instance.GetType().Assembly.Location}");
        }
    }
}