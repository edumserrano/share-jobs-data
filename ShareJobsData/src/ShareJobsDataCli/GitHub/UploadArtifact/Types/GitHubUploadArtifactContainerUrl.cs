namespace ShareJobsDataCli.GitHub.UploadArtifact.Types;

internal record GitHubUploadArtifactContainerUrl
{
    private readonly string _value;

    public GitHubUploadArtifactContainerUrl(string runtimeUrl, string runId)
    {
        runtimeUrl.NotNullOrWhiteSpace();
        runId.NotNullOrWhiteSpace();
        _value = $"{runtimeUrl}_apis/pipelines/workflows/{runId}/artifacts?api-version={GitHubApiVersion.Latest}";
    }

    public static implicit operator string(GitHubUploadArtifactContainerUrl containerUrl)
    {
        return containerUrl._value;
    }

    public override string ToString() => (string)this;
}
