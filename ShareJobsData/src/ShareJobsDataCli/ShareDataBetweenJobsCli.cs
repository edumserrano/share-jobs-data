namespace ShareJobsDataCli;

public class ShareDataBetweenJobsCli
{
    public ShareDataBetweenJobsCli()
    {
        CliApplicationBuilder = new CliApplicationBuilder().AddCommandsFromThisAssembly();
    }

    public CliApplicationBuilder CliApplicationBuilder { get; }

    public ValueTask<int> RunAsync(params string[] args)
    {
        args ??= Array.Empty<string>();
        return CliApplicationBuilder
            .Build()
            .RunAsync(args);
    }
}
