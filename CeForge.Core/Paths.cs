namespace CeForge.Core;

internal static class Paths
{
    public static readonly string ProgramDirectory = Environment.CurrentDirectory;
    public static readonly string ResourcesDirectory = Path.Combine(ProgramDirectory, "Resources");
    public static readonly string SourcesDirectory = Path.Combine(ProgramDirectory, "SourcesTempDirectory");
    public static readonly string GitHubInfoPath = Path.Combine(ResourcesDirectory, "githubCesiumInfo.json");
}