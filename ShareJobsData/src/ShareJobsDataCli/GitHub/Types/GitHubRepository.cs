namespace ShareJobsDataCli.GitHub.Types;

internal sealed record GitHubRepository
{
    private readonly string _value;

    public GitHubRepository(string repository)
    {
        _value = repository.NotNullOrWhiteSpace();
    }

    public static implicit operator string(GitHubRepository repository)
    {
        return repository._value;
    }

    public override string ToString() => (string)this;
}
