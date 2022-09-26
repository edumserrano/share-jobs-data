namespace ShareJobsDataCli.Tests.Auxiliary.Http.ResponseContentFromFiles;

internal static class TestFiles
{
    // use this when the test data file you require is unique to a test in a test class
    public static TestFilepath GetFilepath(
        string endFilepathSegment,
        [CallerFilePath] string sourceFile = "",
        [CallerMemberName] string methodName = "")
    {
        sourceFile = sourceFile.Replace(".cs", string.Empty, StringComparison.OrdinalIgnoreCase);
        var testFilepath = $"{sourceFile}.{methodName}.{endFilepathSegment}";
        return new TestFilepath(testFilepath);
    }

    // use this when you want to share some test files between different tests in the same test class
    public static TestFilepath GetSharedFilepath(
        string endFilepathSegment,
        [CallerFilePath] string sourceFile = "")
    {
        sourceFile = sourceFile.Replace(".cs", string.Empty, StringComparison.OrdinalIgnoreCase);
        var testFilepath = $"{sourceFile}._shared.{endFilepathSegment}";
        return new TestFilepath(testFilepath);
    }
}
