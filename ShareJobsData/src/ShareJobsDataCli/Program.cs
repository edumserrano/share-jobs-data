namespace ShareJobsDataCli;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var githubOutputFile = Environment.GetEnvironmentVariable("GITHUB_OUTPUT") ?? string.Empty;
        //await using var textWriter = new StreamWriter(githubOutputFile, append: true, Encoding.UTF8);
        var textWriter = new StreamWriter(githubOutputFile, append: true, Encoding.UTF8);
        Console.SetOut(textWriter);
        //await textWriter.WriteLineAsync("updated-metrics=21");
        //await textWriter.WriteLineAsync("summary-title=123");
        //await textWriter.WriteLineAsync("summary-details=123");


        var app = new ShareDataBetweenJobsCli();
        return await app.RunAsync(args);
    }
}
