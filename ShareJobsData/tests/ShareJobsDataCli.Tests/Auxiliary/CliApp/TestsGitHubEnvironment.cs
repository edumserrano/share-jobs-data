namespace ShareJobsDataCli.Tests.Auxiliary.CliApp;

internal class TestsGitHubEnvironment : IGitHubEnvironment
{
    public string GitHubActionRuntimeToken => "test-gh-action-runtime-token";

    public string GitHubActionRuntimeUrl => "test-gh-action-runtime-url";

    public string GitHubActionRunId => "test-gh-action-run-id";

    public string GitHubRepository => "source-repo";
}
