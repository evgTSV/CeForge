namespace CeForge.Core.Loader;

public static class ImageCache
{
    public static async IAsyncEnumerable<ImageHash> GetImageHashes()
    {
        yield return await Task.Run(() => new ImageHash());
    }

    public static async Task<CompilerImage> LoadCachedImage(ImageHash hash)
    {
        throw new NotImplementedException();
    }
}