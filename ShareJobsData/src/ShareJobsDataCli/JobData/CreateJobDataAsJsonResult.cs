namespace ShareJobsDataCli.JobData;

internal abstract record CreateJobDataAsJsonResult
{
    private CreateJobDataAsJsonResult()
    {
    }

    public sealed record Ok(JobDataAsJson JobDataAsJson)
        : CreateJobDataAsJsonResult;

    public abstract record Error()
        : CreateJobDataAsJsonResult;

    public sealed record InvalidYml(string ErrorMessage, string Start = "", string End = "")
        : Error;

    public sealed record CannotConvertYmlToJson(string ErrorMessage)
        : Error;

    public static implicit operator CreateJobDataAsJsonResult(JobDataAsJson jobDataAsJson) => new Ok(jobDataAsJson);

    public bool IsOk(
       [NotNullWhen(returnValue: true)] out JobDataAsJson? jobDataAsJson,
       [NotNullWhen(returnValue: false)] out Error? error)
    {
        jobDataAsJson = null;
        error = null;

        if (this is Ok ok)
        {
            jobDataAsJson = ok.JobDataAsJson;
            return true;
        }

        error = (Error)this;
        return false;
    }
}
