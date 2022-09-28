using static ShareJobsDataCli.JobData.CreateJobDataAsJsonResult;

namespace ShareJobsDataCli.JobData;

internal sealed record JobDataAsJson
{
    private readonly JObject _jObject;

    public JobDataAsJson(JObject jObject)
    {
        _jObject = jObject.NotNull();
    }

    public static CreateJobDataAsJsonResult FromYml(string dataAsYmlStr)
    {
        dataAsYmlStr.NotNullOrWhiteSpace();
        var deserializer = new DeserializerBuilder()
            .IgnoreUnmatchedProperties()
            .Build();

#pragma warning disable CA1031 // Do not catch general exception types. Don't know what possible exceptions the yml parsing can throw.
        object ymlObject;
        try
        {
            ymlObject = deserializer.Deserialize<object>(dataAsYmlStr);
        }
        catch (Exception anyYamlException)
        {
            if (anyYamlException is YamlException yamlException)
            {
                return new InvalidYml(yamlException.Message, yamlException.Start.ToString(), yamlException.End.ToString());
            }

            return new InvalidYml(anyYamlException.Message);
        }
#pragma warning restore CA1031 // Do not catch general exception types

#pragma warning disable CA1031 // Do not catch general exception types. Don't know what possible exceptions the yml parsing can throw
        JObject jObject;
        try
        {
            jObject = JObject.FromObject(ymlObject);
        }
        catch (Exception jsonException)
        {
            return new CannotConvertYmlToJson(jsonException.Message);
        }
#pragma warning restore CA1031 // Do not catch general exception types

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
        //var kvp = _jObject.DescendantsAndSelf()
        //   .OfType<JValue>()
        //   .Select(jValue => new { jValue.Path, Value = jValue.Value?.ToString() })
        //   .Where(kvp => kvp.Value is not null)
        //   .Select(kvp => new { kvp.Path, Value = kvp.Value! })
        //   .Select(kvp => new JobDataKeyAndValue(kvp.Path, kvp.Value))
        //   .ToList();
        return new JobDataAsKeysAndValues(kvp);
    }
}
