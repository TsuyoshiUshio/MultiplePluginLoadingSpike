using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace RuntimeAssemblyJsonGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {

            string scaleControllerPath =
                "MultiplePluginLoadingSpike\\bin\\Debug\\net6.0";

            string root = GetRootDirectory();

            IEnumerable<FileInfo> scaleControllerFiles = new DirectoryInfo(Path.Combine(root, scaleControllerPath)).EnumerateFiles().Where(f => f.FullName.EndsWith("dll"));

            JObject runtimeAssembliesJson = new JObject();
            JArray runtimeAssemblies = new JArray();
            foreach (FileInfo scaleControllerFile in scaleControllerFiles)
            {
                var dllName = Path.GetFileNameWithoutExtension(scaleControllerFile.Name);
                var assemblyName = new JObject();
                assemblyName.Add("name", dllName);
                assemblyName.Add("resolutionPolicy", "minorMatchOrLower");
                runtimeAssemblies.Add(assemblyName);
            }
            runtimeAssembliesJson.Add("runtimeAssemblies", runtimeAssemblies);
            string filePath = Path.Combine(root, "MultiplePluginLoadingSpike", "runtimeassemblies.json");
            File.WriteAllText(filePath, runtimeAssembliesJson.ToString());
        }

        private static string GetRootDirectory()
        {
            return Path.GetFullPath(Path.Combine(
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(
                                Path.GetDirectoryName(typeof(Program).Assembly.Location)))))));
        }
    }
}