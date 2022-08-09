namespace ShareJobsDataCli.JobsData;

internal record JobDataKeysAndValues
{
    public JobDataKeysAndValues(List<JobDataKeyAndValue> keysAndValues)
    {
        KeysAndValues = keysAndValues;
    }

    public IReadOnlyList<JobDataKeyAndValue> KeysAndValues { get; }
}

internal record JobDataKeyAndValue
{
    public JobDataKeyAndValue(string key, string value)
    {
        Key = key.NotNullOrWhiteSpace();
        Value = value.NotNull();
    }

    public string Key { get; }

    public string Value { get; }

    // This method allow to deconstruct the type, so you can write any of the following code
    // foreach (var kvp in jobDataKeysAndValues.KeysAndValues) { _ = kvp.Key; }
    // foreach (var (key, value) in jobDataKeysAndValues.KeysAndValues) { _ = key; }
    // https://docs.microsoft.com/en-us/dotnet/csharp/deconstruct?WT.mc_id=DT-MVP-5003978#deconstructing-user-defined-types
    public void Deconstruct(out string key, out string value)
    {
        key = Key;
        value = Value;
    }
}
