namespace CeForge.Core.Loader;

public interface ILoader
{
    public Task<CompilerImage> LoadImage(string path);
}