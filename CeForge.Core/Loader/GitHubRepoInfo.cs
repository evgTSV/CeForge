using Newtonsoft.Json;

namespace CeForge.Core.Loader;

[method: JsonConstructor]
internal class GitHubRepoInfo(string owner, string repo, string? token = null)
{
    [JsonProperty("owner")]
    public string Owner { get; } = owner;

    [JsonProperty("repo")]
    public string Repo { get; } = repo;

    [JsonProperty("auth_token")]
    public string? Token { get; } = token;
}