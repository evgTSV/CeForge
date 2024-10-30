using System.Globalization;
using System.Net.Http.Headers;
using LibGit2Sharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static CeForge.Core.Paths;

namespace CeForge.Core.Loader;

public class GitHubLoader : ILoader
{
    /// <summary>
    /// Fetch and load branch from Cesium repo
    /// </summary>
    public async Task<CompilerImage> LoadImage(string? branchName)
    {
        branchName ??= "main";
        var info = await LoadRepoInfo();
        var repoUrl = $"https://github.com/{info.Owner}/{info.Repo}.git";
        
        var imageHashTask = GetRemoteLatestCommitHash(info, branchName);
        await foreach (var hash in ImageCache.GetImageHashes())
        {
            if (hash.ShaHash == imageHashTask.Result.ShaHash)
                return await ImageCache.LoadCachedImage(hash);
        }

        var imageHash = await imageHashTask;

        if (!Directory.Exists(SourcesDirectory))
            Directory.CreateDirectory(SourcesDirectory);

        string sourcesName = $"{imageHash.ShaHash}";
        string sourcesDirectory = Path.Combine(SourcesDirectory, sourcesName);
        if (Directory.Exists(SourcesDirectory))
            Directory.Delete(SourcesDirectory);
        Directory.CreateDirectory(SourcesDirectory);

        var repo = Repository.Clone(repoUrl, sourcesDirectory, 
            new CloneOptions
            {
                BranchName = branchName
            });
        
        throw new NotImplementedException();
    }

    private async Task<GitHubRepoInfo> LoadRepoInfo()
    {
        string cesiumRepoInfoJson = await File.ReadAllTextAsync(GitHubInfoPath);
        var info = JsonConvert.DeserializeObject<GitHubRepoInfo>(cesiumRepoInfoJson);

        if (info is null)
            throw new LoadImageException($"Cannot to load data from {GitHubInfoPath}");

        return info;
    }

    private async Task<ImageHash> GetRemoteLatestCommitHash(GitHubRepoInfo info, string branchName)
    {
        string url = $"https://api.github.com/repos/{info.Owner}/{info.Repo}/commits/{branchName}";

        using HttpClient client = new HttpClient();
        if (string.IsNullOrEmpty(info.Token))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(info.Token!);

        string json = await client.GetStringAsync(url);

        var tree = JObject.Parse(json);
        string? sha = tree["sha"]?.ToString();
        string? date = tree["commit"]?["commiter"]?["date"]?.ToString();

        if (sha is null || date is null)
            throw new GitHubApiException("Received invalid json schema");

        return new ImageHash(sha, DateTime.Parse(date, null, DateTimeStyles.RoundtripKind));
    }
}