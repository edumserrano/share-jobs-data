using System.IO;

namespace ShareJobsDataCli;

public class ShareDataBetweenJobsCli
{
    public ShareDataBetweenJobsCli()
    {
        var githubOutputFile = Environment.GetEnvironmentVariable("GITHUB_OUTPUT") ?? string.Empty;
        var textWriter = new StreamWriter(githubOutputFile, append: true, Encoding.UTF8);
        textWriter.AutoFlush = true;
        Console.SetOut(textWriter);

        CliApplicationBuilder = new CliApplicationBuilder()
            .AddCommandsFromThisAssembly()
            //.UseConsole(new GitHubStepOutputConsole())
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
