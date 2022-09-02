namespace ShareJobsDataCli.GitHub.Artifacts.DifferentWorkflowRun.DownloadArtifactFile.Types;

internal sealed class GitHubRunId
{
    private readonly string _value;

    public GitHubRunId(string runId)
    {
        _value = runId.NotNullOrWhiteSpace();
    }

    public static implicit operator string(GitHubRunId rundId)
    {
        return rundId._value;
    }

    public override string ToString() => (string)this;
}
