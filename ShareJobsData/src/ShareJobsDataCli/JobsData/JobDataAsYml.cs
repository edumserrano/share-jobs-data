namespace ShareJobsDataCli.JobsData;

internal class JobDataAsYml
{
    private JobDataAsJson? _jobDataJson;
    private readonly object _dataAsYml;

    public JobDataAsYml(string dataAsYmlStr)
    {
        dataAsYmlStr.NotNullOrWhiteSpace();
        var deserializer = new DeserializerBuilder()
            .IgnoreUnmatchedProperties()
            .Build();
        _dataAsYml = deserializer.Deserialize<object>(dataAsYmlStr);
    }

    public JobDataAsJson ToJson()
    {
        return _jobDataJson ??= YamlToJson();

        JobDataAsJson YamlToJson()
        {
            var dataAsJson = JsonConvert.SerializeObject(_dataAsYml, Formatting.Indented);
            return new JobDataAsJson(dataAsJson);
        }
    }
}
