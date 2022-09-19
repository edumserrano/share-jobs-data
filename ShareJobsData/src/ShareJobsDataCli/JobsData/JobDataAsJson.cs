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
        return new JobDataAsKeysAndValues(kvp);
    }

    public static implicit operator string(JobDataAsJson json)
    {
        return json._value;
    }

    public override string ToString() => (string)this;
}
