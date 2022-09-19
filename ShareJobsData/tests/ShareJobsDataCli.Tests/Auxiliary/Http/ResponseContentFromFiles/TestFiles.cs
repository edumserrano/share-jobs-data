namespace ShareJobsDataCli.Tests.Auxiliary.Http.ResponseContentFromFiles;

internal static class TestFiles
{
    public static TestFilepath GetFilepath(
        string endFilepathSegment,
        [CallerFilePath] string sourceFile = "",
        [CallerMemberName] string methodName = "")
    {
        sourceFile = sourceFile.Replace(".cs", string.Empty, StringComparison.OrdinalIgnoreCase);
        var testFilepath = $"{sourceFile}.{methodName}.{endFilepathSegment}";
        return new TestFilepath(testFilepath);
    }
}
