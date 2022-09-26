namespace ShareJobsDataCli.Tests.CliCommands.SetData;

/// <summary>
/// These tests check what happens when a logic violation occurs when running the <see cref="SetDataCommand"/>.
/// </summary>
[Trait("Category", XUnitCategories.LogicFailure)]
[Trait("Category", XUnitCategories.SetDataCommand)]
[UsesVerify]
public class SetDataCommandErrorTests
{
    [Fact]
    public async Task InvalidYml()
    {
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        (var httpClient, var outboundHttpRequests) = TestHttpClient.CreateWithRecorder(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            DataAsYmlStr = TestFiles.GetFilepath("job-data.input.yml").ReadFile(),
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        console.ReadOutputString().ShouldBeEmpty();
        outboundHttpRequests.ShouldBeEmpty();
        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
    }

    [Fact]
    public async Task CannotConvertToJson()
    {
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        (var httpClient, var outboundHttpRequests) = TestHttpClient.CreateWithRecorder(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            DataAsYmlStr = TestFiles.GetFilepath("job-data.input.yml").ReadFile(),
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        console.ReadOutputString().ShouldBeEmpty();
        outboundHttpRequests.ShouldBeEmpty();
        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
    }
}
