namespace CeForge.Core.Loader;

public readonly struct ImageHash(string hash, DateTime timeStamp)
{
    public readonly string ShaHash = hash;
    public readonly DateTime TimeStamp = timeStamp;
}