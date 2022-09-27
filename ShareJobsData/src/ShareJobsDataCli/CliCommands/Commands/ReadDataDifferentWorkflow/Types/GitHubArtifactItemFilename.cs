namespace ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow.Types;

internal sealed record GitHubArtifactItemFilename
{
    private readonly string _value;

    public GitHubArtifactItemFilename(string artifactFilename)
    {
        _value = artifactFilename.NotNullOrWhiteSpace();
    }

    public static implicit operator string(GitHubArtifactItemFilename artifactFilename)
    {
        return artifactFilename._value;
    }

    public override string ToString() => (string)this;
}
