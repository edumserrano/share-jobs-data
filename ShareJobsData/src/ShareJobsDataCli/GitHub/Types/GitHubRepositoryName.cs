namespace ShareJobsDataCli.GitHub.Types;

internal sealed record GitHubRepositoryName
{
    private readonly string _value;

    public GitHubRepositoryName(string repository)
    {
        _value = repository.NotNullOrWhiteSpace();
    }

    public static implicit operator string(GitHubRepositoryName repository)
    {
        return repository._value;
    }

    public override string ToString() => this;
}
