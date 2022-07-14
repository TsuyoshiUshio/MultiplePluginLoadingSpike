using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplePluginLoadingSpike.PluginLoader
{
    public class SystemEnvironment : IEnvironment
    {
        private static readonly Lazy<SystemEnvironment> _instance = new Lazy<SystemEnvironment>(CreateInstance);

        private SystemEnvironment()
        {
        }

        public static SystemEnvironment Instance => _instance.Value;

        private static SystemEnvironment CreateInstance()
        {
            return new SystemEnvironment();
        }

        public string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }

        public void SetEnvironmentVariable(string name, string value)
        {
            Environment.SetEnvironmentVariable(name, value);
        }
    }
}
