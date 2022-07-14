using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplePluginLoadingSpike.PluginLoader
{
    /// <summary>
    /// Provides an environment abstraction to access environment variables, properties and configuration.
    /// </summary>
    public interface IEnvironment
    {
        /// <summary>
        /// Returns the value of an environment variable for the current <see cref="IEnvironment"/>.
        /// </summary>
        /// <param name="name">The environment variable name.</param>
        /// <returns>The value of the environment variable specified by <paramref name="name"/>, or <see cref="null"/> if the environment variable is not found.</returns>
        string GetEnvironmentVariable(string name);

        /// <summary>
        /// Creates, modifies, or deletes an environment variable stored in the current <see cref="IEnvironment"/>
        /// </summary>
        /// <param name="name">The environment variable name.</param>
        /// <param name="value">The value to assign to the variable.</param>
        void SetEnvironmentVariable(string name, string value);
    }
}
