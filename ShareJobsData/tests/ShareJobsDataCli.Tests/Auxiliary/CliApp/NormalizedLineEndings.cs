namespace ShareJobsDataCli.Tests.Auxiliary.CliApp;

internal static class NormalizedLineEndings
{
    private const string CR = "%0D"; //encoded \r
    private const string LF = "%0A"; //encoded \n

    /// <summary>
    /// Ensures that the string only contains encoded Unix line endings.
    /// </summary>
    /// <remarks>
    /// For some tests the app serializes a JSON model and replaces line endings with the GitHub encoded value for line endings
    /// so that the value can be set as a GitHub step output. When the models is serialized the newlines change depending
    /// if the serialization occurs in Windows or Unix.
    ///
    /// To deal with this the tests will normalize encoded line endings from the console output to always be encoded Unix line endings.
    /// This is needed so that the *verified* files used as assertions on the tests work as expected regardless of the OS
    /// where the tests are executed.
    ///
    /// Perhaps the app should do this itself ?
    /// See <see cref="GitHubActionStepOutputExtensions.SanitizeGitHubStepOutput"/>.
    /// </remarks>
    /// <param name="original">The string to replace line endings.</param>
    /// <returns>Same string value bue with newlines always in Unix.</returns>
    public static string NormalizeLineEndingsToUnix(this string original)
    {
        return original.Replace(CR + LF, LF, StringComparison.Ordinal);
    }
}
