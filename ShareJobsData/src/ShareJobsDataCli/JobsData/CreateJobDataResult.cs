using static ShareJobsDataCli.JobsData.CreateJobDataResult;

namespace ShareJobsDataCli.JobsData;

internal sealed class CreateJobDataResult : OneOfBase<JobData, InvalidYml, CannotConvertYmlToJson>
{
    public CreateJobDataResult(OneOf<JobData, InvalidYml, CannotConvertYmlToJson> _)
        : base(_)
    {
    }

    public sealed record InvalidYml(string ErrorMessage, string Start = "", string End = "");

    public sealed record CannotConvertYmlToJson(string ErrorMessage);

    public static implicit operator CreateJobDataResult(JobData _) => new CreateJobDataResult(_);
    public static implicit operator CreateJobDataResult(InvalidYml _) => new CreateJobDataResult(_);
    public static implicit operator CreateJobDataResult(CannotConvertYmlToJson _) => new CreateJobDataResult(_);
}
