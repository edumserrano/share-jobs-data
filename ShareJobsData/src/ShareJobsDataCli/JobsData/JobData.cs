using static ShareJobsDataCli.JobsData.CreateJobDataResult;

namespace ShareJobsDataCli.JobsData;

internal sealed record JobData
{
    private readonly JObject _jObject;

    public JobData(JObject jObject)
    {
        _jObject = jObject.NotNull();
    }

    public static CreateJobDataResult FromYml(string dataAsYmlStr)
    {
        dataAsYmlStr.NotNullOrWhiteSpace();

#pragma warning disable CA1031 // Do not catch general exception types. Don't know what possible exceptions the yml parsing can throw.
        object ymlObject;
        try
        {
            var deserializer = new DeserializerBuilder().Build();
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

        SanitizeMultiLineValues(jObject);
        return new JobData(jObject);
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

    // This removes newline characters from the end of multiline values in the YAML.
    // Example:
    //
    // Name: |
    //   Eduardo The Coding
    //   Addict Serrano
    // Gender: Male
    //
    // results in a yaml Dictionary<object,object> where the key 'Name' has the value 'Eduardo The Coding\nAddict Serrano\n'
    //
    // After this method, the value for the key 'Name' becomes 'Eduardo The Coding\nAddict Serrano'.
    private static void SanitizeMultiLineValues(JObject jObject)
    {
        var jValues = jObject.DescendantsAndSelf().OfType<JValue>();
        foreach (var jValue in jValues)
        {
            if (jValue.Value?.ToString() is { } valueAsString)
            {
                jValue.Value = valueAsString
                    .TrimEnd('\n')
                    .TrimEnd('\r');
            }
        }
    }
}
