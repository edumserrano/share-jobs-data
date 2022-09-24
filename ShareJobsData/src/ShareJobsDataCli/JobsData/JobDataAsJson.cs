namespace ShareJobsDataCli.JobsData;

internal sealed record JobDataAsJson
{
    private readonly string _value;

    public JobDataAsJson(string json)
    {
        _value = json.NotNullOrWhiteSpace();
    }

    public JobDataAsKeysAndValues ToKeyValues()
    {
        var obj = JObject.Parse(_value);
        var kvp = obj.DescendantsAndSelf()
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
        //var kvp = obj.DescendantsAndSelf()
        //    .OfType<JValue>()
        //    .Select(jValue => new { jValue.Path, Value = jValue.Value?.ToString() })
        //    .Where(kvp => kvp.Value is not null)
        //    .Select(kvp => new { kvp.Path, Value = kvp.Value! })
        //    .Select(kvp => new JobDataKeyAndValue(kvp.Path, kvp.Value))
        //    .ToList();
        return new JobDataAsKeysAndValues(kvp);
    }

    public static implicit operator string(JobDataAsJson json)
    {
        return json._value;
    }

    public override string ToString() => (string)this;
}
