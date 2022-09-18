namespace ShareJobsDataCli.Tests.Auxiliary;

internal static class FakeInMemoryConsoleExtensions
{
    public static string ReadAllAsString(this FakeInMemoryConsole console)
    {
        var output = console.ReadOutputString();
        var error = console.ReadErrorString();
        return error + output;
    }
}
