using CeForge.Core.Loader;

namespace CeForge.Core.BuildingSystem;

public class BuildFailedException(string message, CompilerImage image) : Exception(message)
{
    public CompilerImage TryToBuildImage => image;
}