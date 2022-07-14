using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplePluginLoadingSpike.PluginLoader
{
    internal sealed class ScriptRuntimeAssembly
    {
        public string Name { get; set; }

        public string ResolutionPolicy { get; set; }
    }
}
