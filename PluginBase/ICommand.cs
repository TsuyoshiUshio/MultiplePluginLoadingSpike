using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PluginBase
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        void Initialize(IDictionary<string, string> context, ILoggerFactory loggerFactory);

        void Execute();
    }
}
