namespace ShareJobsDataCli.Tests.CliCommands.SetData.DependencyErrors;

/// <summary>
/// These tests check what happens when the upload artifact file HTTP dependency of the <see cref="SetDataCommand"/> fails.
/// </summary>
[Trait("Category", XUnitCategories.DependencyFailure)]
[Trait("Category", XUnitCategories.SetDataCommand)]
[UsesVerify]
public class FailedHttpToUploadArtifactFileTests
{
}
