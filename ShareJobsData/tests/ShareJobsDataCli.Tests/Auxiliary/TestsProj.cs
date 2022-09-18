namespace ShareJobsDataCli.Tests.Auxiliary;

internal static class TestsProj
{
    private static string GetSolutionDir()
    {
        var dir = Directory.GetCurrentDirectory();
        while (dir != null)
        {
            var files = Directory.GetFiles(dir, "*.sln");
            if (files?.Length > 0)
            {
                return dir;
            }

            dir = Directory.GetParent(dir)?.FullName;
        }

        if (dir is null)
        {
            throw new InvalidOperationException("Couldn't find the solution directory");
        }

        return dir;
    }

    public static string GetAbsoluteFilepath(string relativePathToSlnFile)
    {
        var slnDir = GetSolutionDir();
        return Path.Combine(slnDir, relativePathToSlnFile);
    }
}
