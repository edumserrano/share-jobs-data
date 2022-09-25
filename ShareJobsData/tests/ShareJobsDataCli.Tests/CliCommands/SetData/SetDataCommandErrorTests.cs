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
        (var httpClient, var outboundHttpRequests) = TestHttpClientFactory.Create(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            DataAsYmlStr = TestFiles.GetFilepath("job-data.input.yml").ReadFile(),
        };
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(() => command.ExecuteAsync(console).AsTask());

        await Verify(exception.Message).AppendToMethodName("console-output");
        await Verify(outboundHttpRequests).AppendToMethodName("outbound-http");
    }

    [Fact]
    public async Task CannotConvertToJson()
    {
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        (var httpClient, var outboundHttpRequests) = TestHttpClientFactory.Create(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            DataAsYmlStr = TestFiles.GetFilepath("job-data.input.yml").ReadFile(),
        };
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(() => command.ExecuteAsync(console).AsTask());

        await Verify(exception.Message).AppendToMethodName("console-output");
        await Verify(outboundHttpRequests).AppendToMethodName("outbound-http");
    }
}
