namespace ShareJobsDataCli.Tests.Auxiliary.Verify;

// This class is used to make sure that the tests can read absolute file paths when running in a remote
// test environment.
// The Verify library added support for running in remote test environments via "Map local build path during WSL run #729": https://github.com/VerifyTests/Verify/pull/729.
// However the tests I have will try to use an absolute path to read test data. This absolute path will not work when in a remote test environment
// unless I map it to the remote test environment path.
//
// I looked at how Verify solved this on "Map local build path during WSL run #729" and I implemented something similar but that works just
// for my scenario. Furthermore, I haven't even tested if this would work on a mac running in a test environment. It does work for a windows running
// on a test environment.
//
// The Verify implementation is based on the VirtualizedRunHelper class: https://github.com/VerifyTests/Verify/blob/53f1ac4edd429fce6102655e9f6dc981736e59a5/src/Verify/VirtualizedRunHelper.cs
internal sealed class RemoteTestEnvironment
{
    private static readonly Lazy<RemoteTestEnvironment> _instance = new Lazy<RemoteTestEnvironment>(() => new RemoteTestEnvironment());
    private readonly string _solutionDirectory = string.Empty;
    private readonly string _remoteTestEnvironmentSolutionDir = string.Empty;

    private readonly char[] _separators =
    {
        '\\',
        '/',
    };

    private RemoteTestEnvironment()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        // WSL remote testing paths mount to /mnt/<drive>/...
        // docker remote testing mount to /mnt/approot/...
        IsRunningInRemoteTestEnvironment = currentDirectory.StartsWith("/mnt/", StringComparison.Ordinal);
        if (!IsRunningInRemoteTestEnvironment)
        {
            return;
        }

        var assembly = Assembly.GetExecutingAssembly();
        _solutionDirectory = GetSolutionDirectory(assembly);
        _remoteTestEnvironmentSolutionDir = GetRemoteTestEnvironmentSolutionDirectory(currentDirectory, _solutionDirectory);
    }

    public static RemoteTestEnvironment Instance => _instance.Value;

    public bool IsRunningInRemoteTestEnvironment { get; }

    public string GetRemoteTestEnvironmentPath(string absolutePathToMap)
    {
        var separatorToReplace = _separators.First(x => x != Path.DirectorySeparatorChar);
        var relativePath = absolutePathToMap
            .Replace(_solutionDirectory, string.Empty, StringComparison.Ordinal)
            .Replace($"{separatorToReplace}", $"{Path.DirectorySeparatorChar}", StringComparison.Ordinal);
        var mappedPath = Path.Combine(_remoteTestEnvironmentSolutionDir, relativePath);
        return mappedPath;
    }

    private string GetRemoteTestEnvironmentSolutionDirectory(string currentDirectory, string solutionDirectory)
    {
        // if we define:
        // - the run platform as the platform the tests are running on
        // - the host platform as the platform that is triggering the test run
        // then when we are not using remote test environments:
        // - the run platform and host platform are the same.
        // and when we are using remote test environments:
        // - the run platform is whatever remote test environment was defined, such as docker container or wsl or etc.
        // - the host platform is the platform triggering the test run, such as windows or mac or etc.
        //
        // The below should only be used when we already now we are running in a remote test environment.
        // In a remote test environment:
        // - the solutionDirectory will always be a directory in the host platform (ie.: windows)
        // - the currentDirectory will always be a directory in the the run platform (ie.: docker using linux)
        //
        // So if we want to find the remote test environment solution directory, we need to find the folder on
        // the run platform directory that should be the solution folder which we can derive from the the last folder
        // on the host platform solution directory.
        // As an example, when running tests, given:
        // - currentDirectory is /mnt/approot/ShareJobsData/tests/ShareJobsDataCli.Tests/bin/Debug/net7.0 (directory from docker running a linux distro)
        // - solutionDirectory is D:\Dev\ShareJobsData (directory from Windows)
        // Then the remote test environment solution folder should be:
        // - /mnt/approot/ShareJobsData

        var solutionDirectoryFolders = solutionDirectory.Split(_separators, StringSplitOptions.RemoveEmptyEntries);
        var solutionFolder = solutionDirectoryFolders[^1];
        var solutionFolderStartIndex = currentDirectory.IndexOf(solutionFolder, StringComparison.Ordinal);
        var solutionFolderEndIndex = solutionFolderStartIndex + solutionFolder.Length;
        return currentDirectory[..solutionFolderEndIndex];
    }

    private static string GetSolutionDirectory(Assembly assembly)
    {
        // This method takes advantage of the fact that the Verify NuGet package adds custom attributes to the test assembly.
        // From https://github.com/VerifyTests/Verify/blob/53f1ac4edd429fce6102655e9f6dc981736e59a5/docs/mdsource/scrubbers.source.md?plain=1#L29-L34 :
        // "
        // The solution and project directory replacement functionality is achieved by adding attributes to the
        // target assembly at compile time. For any project that references Verify, the following attributes will be added:
        //
        // [assembly: AssemblyMetadata("Verify.ProjectDirectory", "C:\Code\TheSolution\Project\")]
        // [assembly: AssemblyMetadata("Verify.SolutionDirectory", "C:\Code\TheSolution\")]
        // "

        const string solutionDirectoryKey = "Verify.SolutionDirectory";
        foreach (var attribute in Attribute.GetCustomAttributes(assembly, typeof(AssemblyMetadataAttribute), inherit: false))
        {
            var metaData = (AssemblyMetadataAttribute)attribute;
            if (!string.Equals(metaData.Key, solutionDirectoryKey, StringComparison.Ordinal))
            {
                continue;
            }

            if (metaData.Value is not null)
            {
                return metaData.Value;
            }
        }

        throw new InvalidOperationException($"Null value for `AssemblyMetadataAttribute` named `{solutionDirectoryKey}`.");
    }
}
