namespace ShareJobsDataCli.Tests.CliCommands.ReadDataDifferentWorkflow;

[Trait("Category", XUnitCategories.Commands)]
public class ReadDataFromDifferentGitHubWorkflowCommandTests
{
    [Fact]
    public void Test1()
    {
        //var someHttpCallMock = new HttpResponseMessageMockBuilder()
        //    .Where(httpRequestMessage => httpRequestMessage.RequestUri!.AbsolutePath.Equals($"repos/{repoName}/actions/artifacts"))
        //    .RespondWith(httpRequestMessage =>
        //    {
        //        var 
        //        return new HttpResponseMessage(HttpStatusCode.OK)
        //        {
        //            Content = new StringContent("some mocked value")
        //        };
        //    })
        //    .Build();
        //var anotherHttpCallMock = new HttpResponseMessageMockBuilder()
        //    .Where(httpRequestMessage => httpRequestMessage.RequestUri.PathAndQuery.Equals("/another-http-call"))
        //    .RespondWith(httpRequestMessage => new HttpResponseMessage(HttpStatusCode.Accepted)
        //    {
        //        Content = new StringContent("another mocked value")
        //    })
        //    .Build();



        //var command = new ReadDataFromDifferentGitHubWorkflowCommand(httpClient, file)
        //{
        //    AuthToken = "auth-token",
        //    Repo = "repo-name",
        //    RunId = "run-id",
        //    JobName = "Markdown link check",
        //    StepName = "Markdown link check",
        //};
        //using var console = new FakeInMemoryConsole();
        //await command.ExecuteAsync(console);
        //var output = console
        //    .ReadOutputString()
        //    .UnEscapeGitHubStepOutput();

    }
}
