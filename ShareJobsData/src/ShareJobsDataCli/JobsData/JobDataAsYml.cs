namespace ShareJobsDataCli.JobsData;

internal class JobDataAsYml
{
    private JobDataAsJson? _jobDataJson;
    private readonly object _dataAsYml;

    public JobDataAsYml(string yml)
    {
        var deserializer = new DeserializerBuilder()
            .IgnoreUnmatchedProperties()
            .Build();
        _dataAsYml = deserializer.Deserialize<object>(yml);
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
