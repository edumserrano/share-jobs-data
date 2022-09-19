namespace ShareJobsDataCli.Tests.Auxiliary.Http.ResponseContentFromFiles;

internal sealed record TestFilepath
{
    private readonly string _value;

    public TestFilepath(string testFilepath)
    {
        _value = testFilepath;
    }

    public static implicit operator string(TestFilepath testFilepath)
    {
        return testFilepath._value;
    }
}
