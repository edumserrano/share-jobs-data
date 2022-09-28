namespace ShareJobsDataCli.JobsData;

internal abstract record CreateJobDataResult
{
    private CreateJobDataResult()
    {
    }

    public sealed record Ok(JobData JobDataAsJson)
        : CreateJobDataResult;

    public abstract record Error()
        : CreateJobDataResult;

    public sealed record InvalidYml(string ErrorMessage, string Start = "", string End = "")
        : Error;

    public sealed record CannotConvertYmlToJson(string ErrorMessage)
        : Error;

    public static implicit operator CreateJobDataResult(JobData jobDataAsJson) => new Ok(jobDataAsJson);

    public bool IsOk(
       [NotNullWhen(returnValue: true)] out JobData? jobDataAsJson,
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
