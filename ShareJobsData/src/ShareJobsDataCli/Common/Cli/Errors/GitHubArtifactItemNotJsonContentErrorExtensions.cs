namespace ShareJobsDataCli.Common.Cli.Errors;

internal static class GitHubArtifactItemNotJsonContentErrorExtensions
{
    public static string AsErrorMessage(this GitHubArtifactItemNotJsonContent notJsonContent)
    {
        notJsonContent.NotNull();
        var artifactItemContent = string.IsNullOrEmpty(notJsonContent.ItemContent)
            ? "<empty>"
            : $"{Environment.NewLine}---START---{Environment.NewLine}{notJsonContent.ItemContent}{Environment.NewLine}---END---";
        return $"Content from downloaded artifact item must be JSON. JSON error: '{notJsonContent.JsonReaderErrorMessage}'. JSON response: {artifactItemContent}";
    }
}
