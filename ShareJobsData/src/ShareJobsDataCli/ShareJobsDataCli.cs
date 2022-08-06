namespace ShareJobsDataCli;

public class ShareJobsDataCli
{
    public ShareJobsDataCli()
    {
        CliApplicationBuilder = new CliApplicationBuilder().AddCommandsFromThisAssembly();
    }

    public CliApplicationBuilder CliApplicationBuilder { get; }

    public ValueTask<int> RunAsync(IReadOnlyList<string> args = default!)
    {
        args ??= Array.Empty<string>();
        return CliApplicationBuilder
            .Build()
            .RunAsync(args);
    }
}
