namespace MultiplePluginLoadingSpike.PluginLoader
{
    public class RuntimeAsset
    {
        public RuntimeAsset(string rid, string path, string assemblyVersion)
        {
            Rid = rid;
            Path = path;

            if (!string.IsNullOrEmpty(assemblyVersion))
            {
                AssemblyVersion = new Version(assemblyVersion);
            }
        }

        public string Rid { get; }

        public string Path { get; }

        public Version AssemblyVersion { get; }

        private string Display => $"({Rid ?? "no RID"}) - {Path} - {AssemblyVersion}";
    }
}