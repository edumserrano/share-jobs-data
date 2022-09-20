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
            .OfType<JValue>()
            .Select(jValue => new { jValue.Path, Value = jValue.Value?.ToString() })
            .Where(kvp => kvp.Value is not null)
            .Select(kvp => new { kvp.Path, Value = kvp.Value! })
            .Select(kvp => new JobDataKeyAndValue(kvp.Path, kvp.Value))
            .ToList();
        return new JobDataAsKeysAndValues(kvp);
    }

    public static implicit operator string(JobDataAsJson json)
    {
        return json._value;
    }

    public override string ToString() => (string)this;
}
