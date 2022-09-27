namespace ShareJobsDataCli.Tests.Auxiliary.Verify;

internal static class VerifyExtensions
{
    public static SettingsTask AppendToMethodName(
        this SettingsTask settingsTask,
        string appendValue,
        [CallerMemberName] string methodName = "")
    {
        return settingsTask.UseMethodName($"{methodName}.{appendValue}");
    }

    public static void ScrubAppName(this VerifySettings settings)
    {
        settings.ScrubLinesWithReplace(line =>
        {
            // when running on windows the app name is set to 'testhost'
            // when running on unix the app name is set to 'dotnet testhost.dll'
            // this scrubber makes sure the output is equal on both
            return line
                .Replace("dotnet testhost.dll", "{scrubbed app name}", StringComparison.OrdinalIgnoreCase)
                .Replace("testhost", "{scrubbed app name}", StringComparison.OrdinalIgnoreCase);
        });
    }
}
