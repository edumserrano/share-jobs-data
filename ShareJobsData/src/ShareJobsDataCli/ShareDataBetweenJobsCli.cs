namespace ShareJobsDataCli;

public class ShareDataBetweenJobsCli
{
    public ShareDataBetweenJobsCli()
    {
        CliApplicationBuilder = new CliApplicationBuilder()
            .AddCommandsFromThisAssembly()
            .UseConsole(new GitHubStepOutputConsole())
            ;
    }

    public CliApplicationBuilder CliApplicationBuilder { get; }

    public ValueTask<int> RunAsync(params string[] args)
    {
        return CliApplicationBuilder
            .Build()
            .RunAsync(args);
    }
}
