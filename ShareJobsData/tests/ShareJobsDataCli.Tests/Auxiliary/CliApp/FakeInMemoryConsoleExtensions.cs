namespace ShareJobsDataCli.Tests.Auxiliary.CliApp;

internal static class FakeInMemoryConsoleExtensions
{
    public static string ReadAllAsString(this FakeInMemoryConsole console)
    {
        // don't know how I would test that the errors have a red foreground =/
        var output = console.ReadOutputString();
        var error = console.ReadErrorString();
        return error + output;
    }
}
