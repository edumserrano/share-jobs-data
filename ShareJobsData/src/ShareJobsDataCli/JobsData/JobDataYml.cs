namespace ShareJobsDataCli.JobsData;

internal sealed class JobDataYml
{
    private JobDataJson? _jobDataJson;
    private readonly object _dataAsYml;

    public JobDataYml(string yml)
    {
        var deserializer = new DeserializerBuilder()
            .IgnoreUnmatchedProperties()
            .Build();
        _dataAsYml = deserializer.Deserialize<object>(yml);
    }

    public JobDataJson ToJson()
    {
        return _jobDataJson ??= YamlToJson();

        JobDataJson YamlToJson()
        {
            var dataAsJson = JsonConvert.SerializeObject(_dataAsYml, Formatting.Indented);
            return new JobDataJson(dataAsJson);
        }
    }
}
