using System.Text.RegularExpressions;

namespace ShareJobsDataCli.Tests.CliIntegration.ReadDataDifferentWorkflow;

/// <summary>
/// These tests make sure that the CLI interface is as expected.
/// IE: if the command name changes or the options change then these tests would pick that up.
/// These tests also test the <see cref="IBindingValidator"/> validators of the command options.
/// </summary>
[Trait("Category", XUnitCategories.Integration)]
[Trait("Category", XUnitCategories.ReadDataFromDifferentGitHubWorkflowCommand)]
[UsesVerify]
public class CliIntegrationTests
{
    /// <summary>
    /// Tests that if no arguments are passed the CLI returns the help text.
    /// </summary>
    [Fact]
    public async Task NoArguments()
    {
        using var console = new FakeInMemoryConsole();
        var app = new ShareDataBetweenJobsCli();
        app.CliApplicationBuilder.UseConsole(console);
        await app.RunAsync();
        var output = console.ReadOutputString();

        var settings = new VerifySettings();
        settings.ScrubAppName();
        await Verify(output, settings);
    }
}

internal static class ScrubExtensions
{
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

