namespace ShareJobsDataCli.JobsData;

internal sealed record JobDataJson
{
    private readonly string _value;

    public JobDataJson(string json)
    {
        _value = json.NotNullOrWhiteSpace();
    }

    public static implicit operator string(JobDataJson json)
    {
        return json._value;
    }

    public JobDataKeysAndValues ToKeyValues()
    {
        var obj = JObject.Parse(_value);
        var kvp = obj.DescendantsAndSelf()
            .OfType<JProperty>()
            .Where(jp => jp.Value is JValue)
            .Select(jp => new JobDataKeyAndValue(jp.Path, jp.Value.ToString()))
            .ToList();
        return new JobDataKeysAndValues(kvp);
    }

    public override string ToString() => (string)this;
}
