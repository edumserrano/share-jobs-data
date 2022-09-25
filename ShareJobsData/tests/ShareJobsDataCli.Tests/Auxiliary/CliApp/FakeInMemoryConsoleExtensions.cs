using System.Reflection;

namespace ShareJobsDataCli.Tests.Auxiliary.CliApp;

internal static class FakeInMemoryConsoleExtensions
{
    public static string ReadAllAsString(this FakeInMemoryConsole console)
    {
        // TODO don't know how I would test that the errors have a red foreground =/
        var output = console.ReadOutputString();
        var error = console.ReadErrorString();
        return error + output;
    }

    public static SettingsTask VerifyOutput(
        this FakeInMemoryConsole console,
        VerifySettings verifySettings = default!,
        [CallerMemberName] string methodName = "")
    {
        verifySettings ??= new VerifySettings();
        var errorString = console.ReadOutputString();
        return Verify(errorString, verifySettings).AppendToMethodName("console-output", methodName);
    }

    public static SettingsTask VerifyError(
        this FakeInMemoryConsole console,
        VerifySettings verifySettings = default!,
        [CallerMemberName] string methodName = "")
    {
        verifySettings ??= new VerifySettings();
        var errorString = console.ReadErrorString();
        return Verify(errorString, verifySettings).AppendToMethodName("console-error", methodName);
    }
}
