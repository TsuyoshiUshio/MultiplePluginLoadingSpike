using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplePluginLoadingSpike.PluginLoader
{
    internal class RuntimeAssembliesInfo
    {
        private readonly IEnvironment _environment;
        private Lazy<Dictionary<string, ScriptRuntimeAssembly>> _runtimeAssemblies;
        private object _loadSyncRoot = new object();
        private bool? _relaxedUnification;

        public RuntimeAssembliesInfo()
            : this(SystemEnvironment.Instance)
        {
        }

        public RuntimeAssembliesInfo(IEnvironment instance)
        {
            _environment = instance;
            _runtimeAssemblies = new Lazy<Dictionary<string, ScriptRuntimeAssembly>>(GetRuntimeAssemblies);
        }

        public Dictionary<string, ScriptRuntimeAssembly> Assemblies => _runtimeAssemblies.Value;

        private Dictionary<string, ScriptRuntimeAssembly> GetRuntimeAssemblies()
        {
            lock (_loadSyncRoot)
            {
                string manifestName = "runtimeassemblies.json";

                return DependencyHelper.GetRuntimeAssemblies(manifestName);
            }
        }

        public bool ResetIfStale()
        {
            lock (_loadSyncRoot)
            {
                if (_relaxedUnification != null)
                {
                    _runtimeAssemblies = new Lazy<Dictionary<string, ScriptRuntimeAssembly>>(GetRuntimeAssemblies);

                    return true;
                }
            }

            return false;
        }
    }
}
