namespace ShareJobsDataCli.Tests.Auxiliary.VerifyExtensions;

public static class ModuleInitializer
{
    // When modifying settings at the both global level it should be done using a Module Initializer.
    // See https://github.com/VerifyTests/Verify#static-settings.
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyDiffPlex.Initialize();
        VerifierSettings.DisableRequireUniquePrefix(); // allows me to have Theory tests that map to the same verified file
    }
}
