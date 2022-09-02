namespace ShareJobsDataCli.GitHub.Artifacts.Types;

internal record GitHubArtifactContainerUrl
{
    private readonly string _value;

    public GitHubArtifactContainerUrl(string runtimeUrl, string runId)
    {
        runtimeUrl.NotNullOrWhiteSpace();
        runId.NotNullOrWhiteSpace();
        _value = $"{runtimeUrl}_apis/pipelines/workflows/{runId}/artifacts?api-version={GitHubApiVersion.Latest}";
    }

    public static implicit operator string(GitHubArtifactContainerUrl containerUrl)
    {
        return containerUrl._value;
    }

    public override string ToString() => (string)this;
}
