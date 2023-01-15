namespace ShareJobsDataCli.Tests.Auxiliary.CliApp;

internal sealed class TestsGitHubEnvironment : IGitHubEnvironment
{
    public string GitHubActionRuntimeToken => "____xbk9w02Bwt7WD29DzY3xRQVhdqLcGT1eS4lc";

    public string GitHubActionRuntimeUrl => "https://pipelines.actions.githubusercontent.com/pasYWZMKAGeorzjszgve9v6gJE03WMQ2NXKn6YXBa7i57yJ5WP/";

    public string GitHubActionRunId => "3085101792";

    public string GitHubRepository => "source-repo";

    public string GitHubOutputFile => "GitHubOutputFile";

    public string GitHubOutputFile2 => "GitHubOutputFile2";
}
