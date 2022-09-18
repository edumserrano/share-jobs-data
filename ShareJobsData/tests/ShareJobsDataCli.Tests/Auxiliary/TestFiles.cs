namespace ShareJobsDataCli.Tests.Auxiliary;

internal static class TestFiles
{
    public static string GetAbsoluteFilepath(
        string endFilepathSegment,
        [CallerFilePath] string sourceFile = "",
        [CallerMemberName] string methodName = "")
    {
        sourceFile = sourceFile.Replace(".cs", string.Empty, StringComparison.OrdinalIgnoreCase);
        return $"{sourceFile}.{methodName}.{endFilepathSegment}";
    }
}
