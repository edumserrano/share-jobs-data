namespace ShareJobsDataCli.Tests.Auxiliary.Http.ResponseContentFromFiles;

internal static class TestFilepathExtensions
{
    public static string ReadFile(this TestFilepath testFilepath)
    {
        return File.ReadAllText(testFilepath);
    }

    public static StringContent ReadFileAsStringContent(this TestFilepath testFilepath)
    {
        var responseContentAsString = File.ReadAllText(testFilepath);
        return new StringContent(responseContentAsString);
    }

    public static StreamContent ReadFileAsAzipContent(this TestFilepath testFilepath)
    {
        var zipFileStream = File.OpenRead(testFilepath);
        var streamContent = new StreamContent(zipFileStream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Zip);
        return streamContent;
    }
}
