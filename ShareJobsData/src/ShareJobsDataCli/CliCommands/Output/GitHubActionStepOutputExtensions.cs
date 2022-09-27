namespace ShareJobsDataCli.CliCommands.Output;

internal static class GitHubActionStepOutputExtensions
{
    public static Task WriteAsync(this GitHubActionStepOutput gitHubActionStepOutput, GitHubArtifactItemJsonContent gitHubArtifactItemJsonContent)
    {
        var jobDataAsJson = new JobDataAsJson(gitHubArtifactItemJsonContent.AsJObject());
        return gitHubActionStepOutput.WriteAsync(jobDataAsJson);
    }

    public static Task WriteAsync(this GitHubActionStepOutput gitHubActionStepOutput, JobDataAsJson jobDataAsJson)
    {
        var jobDataAsKeysAndValues = jobDataAsJson.AsKeyValues();
        return gitHubActionStepOutput.WriteAsync(jobDataAsKeysAndValues);
    }
}
