namespace ShareJobsDataCli.Tests.Features.SetData;

/// <summary>
/// These tests check what happens when a logic violation occurs when running the <see cref="SetDataCommand"/>.
/// </summary>
[Trait("Category", XUnitCategories.LogicFailure)]
[Trait("Category", XUnitCategories.SetDataCommand)]
public sealed class SetDataCommandErrorTests
{
    /// <summary>
    /// Tests that the <see cref="SetDataCommand"/> shows expected error message when the --data is invalid YAML.
    /// </summary>
    [Fact]
    public async Task InvalidYml()
    {
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        var (httpClient, outboundHttpRequests) = TestHttpClient.CreateWithRecorder(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            DataAsYmlStr = TestFiles.GetFilepath("job-data.input.yml").ReadFile(),
        };
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(command.ExecuteAsync(console).AsTask());

        console.ReadOutputString().ShouldBeEmpty();
        outboundHttpRequests.ShouldBeEmpty();
        var output = console.ReadAllAsString();
        await Verify(exception.Message)
            // this scrubber is required due to line ending differences in Windows vs Unix. This verified file shows Idx:30 when running on Windows
            // and Idx:29 when running on Unix. Since the app is not doing any normalization of line endings on the output I'll do this for now
            .ScrubLinesWithReplace(line => line.Replace("Idx: 30", "{scrubbed line ending idx}", StringComparison.OrdinalIgnoreCase))
            .ScrubLinesWithReplace(line => line.Replace("Idx: 29", "{scrubbed line ending idx}", StringComparison.OrdinalIgnoreCase))
            .AppendToMethodName("console-output");
    }

    /// <summary>
    /// Tests that the <see cref="SetDataCommand"/> shows expected error message when the --data is cannot be converted
    /// from YAML to JSON.
    /// </summary>
    [Fact]
    public async Task CannotConvertToJson()
    {
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        var (httpClient, outboundHttpRequests) = TestHttpClient.CreateWithRecorder(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            DataAsYmlStr = TestFiles.GetFilepath("job-data.input.yml").ReadFile(),
        };
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(command.ExecuteAsync(console).AsTask());

        console.ReadOutputString().ShouldBeEmpty();
        outboundHttpRequests.ShouldBeEmpty();
        await Verify(exception.Message).AppendToMethodName("console-output");
    }

    /// <summary>
    /// Tests that the <see cref="SetDataCommand"/> shows expected error message when the --output is invalid.
    /// </summary>
    [Fact]
    public async Task InvalidOutput()
    {
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        var (httpClient, outboundHttpRequests) = TestHttpClient.CreateWithRecorder(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            DataAsYmlStr = TestFiles.GetFilepath("job-data.input.yml").ReadFile(),
            Output = "not-valid-value",
        };
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(command.ExecuteAsync(console).AsTask());

        console.ReadOutputString().ShouldBeEmpty();
        outboundHttpRequests.ShouldBeEmpty();
        await Verify(exception.Message).AppendToMethodName("console-output");
    }
}
