using System.Diagnostics;

namespace CleanUp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Update the following directory for each plugin
            string[] pluginPaths =
            {
                "ServiceBus4Plugin\\bin\\Debug\\net6.0",
                "ServiceBus5Plugin\\bin\\Debug\\net6.0"
            };

            string scaleControllerPath =
                "MultiplePluginLoadingSpike\\bin\\Debug\\net6.0";

            string root = GetRootDirectory();

            IEnumerable<FileInfo> scaleControllerFiles = new DirectoryInfo(Path.Combine(root, scaleControllerPath)).EnumerateFiles();

            // Find duplicated dlls for each plugin and ScaleController 
            IEnumerable<string> filesToBeRemoved = pluginPaths.SelectMany(
                pluginPath =>
                {
                    var pluginFiles = new DirectoryInfo(Path.Combine(root, pluginPath)).EnumerateFiles();
                    return scaleControllerFiles.Join(
                        pluginFiles,
                        scaleControllerFile => new { scaleControllerFile.Name, FileVersionInfo.GetVersionInfo(scaleControllerFile.FullName).FileMajorPart },
                        pluginFile => new { pluginFile.Name, FileVersionInfo.GetVersionInfo(pluginFile.FullName).FileMajorPart },
                        (scaleControllerFile, pluginFile) => pluginFile.FullName
                    );
                });

            Console.WriteLine("Remove Duplicated Dlls from Plugin directories");
            foreach (var duplicatedFile in filesToBeRemoved)
            {
                Console.WriteLine($"Duplicate: {duplicatedFile}  Deleted it.");
                File.Delete(duplicatedFile);
            }
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