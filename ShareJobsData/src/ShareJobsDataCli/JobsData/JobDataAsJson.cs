namespace ShareJobsDataCli.JobsData;

internal sealed record JobDataAsJson
{
    private readonly JObject _jObject;

    public JobDataAsJson(JObject jObject)
    {
        _jObject = jObject.NotNull();
    }

    public static CreateJobDataAsJsonResult FromYml(string dataAsYmlStr)
    {
        // TODO test to see if I need try catches to return error states
        dataAsYmlStr.NotNullOrWhiteSpace();
        var deserializer = new DeserializerBuilder()
            .IgnoreUnmatchedProperties()
            .Build();
        var ymlObject = deserializer.Deserialize<object>(dataAsYmlStr);
        var jObject = JObject.FromObject(ymlObject);
        return new JobDataAsJson(jObject);
    }

    public string AsJson()
    {
        return _jObject.ToString(Formatting.Indented);
    }

    public JobDataAsKeysAndValues AsKeyValues()
    {
        var kvp = _jObject.DescendantsAndSelf()
            .OfType<JProperty>()
            .Where(jp => jp.Value is JValue)
            .Select(jp => new JobDataKeyAndValue(jp.Path, jp.Value.ToString()))
            .ToList();
        // This supports lists
        // however for any property that has a . in it after the step outputs like steps.read.outputs.addresses.home.street
        // the value cannot be read in the action so I would need to adjust the path
        // perhaps replace '.' with '_' ?
        // If I do the replacement then I should actually have an output mode, either strict json or github json
        // this would allow ppl to use strict json and consumer the output using ${{ toJSON(steps.read.outputs) }}
        // and converting the the output to a json object
        // var kvp = obj.DescendantsAndSelf()
        //    .OfType<JValue>()
        //    .Select(jValue => new { jValue.Path, Value = jValue.Value?.ToString() })
        //    .Where(kvp => kvp.Value is not null)
        //    .Select(kvp => new { kvp.Path, Value = kvp.Value! })
        //    .Select(kvp => new JobDataKeyAndValue(kvp.Path, kvp.Value))
        //    .ToList();
        return new JobDataAsKeysAndValues(kvp);
    }
}

internal abstract record CreateJobDataAsJsonResult
{
    private CreateJobDataAsJsonResult()
    {
    }

    public record Ok(JobDataAsJson JobDataAsJson)
        : CreateJobDataAsJsonResult;

    public abstract record Error()
        : CreateJobDataAsJsonResult;

    // TODO need a test to see if I need this
    public record InvalidYml()
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
