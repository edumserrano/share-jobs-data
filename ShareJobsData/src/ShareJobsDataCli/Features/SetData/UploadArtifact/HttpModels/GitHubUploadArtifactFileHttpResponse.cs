namespace ShareJobsDataCli.Features.SetData.UploadArtifact.HttpModels;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via JSON generic type deserialization.
internal sealed record GitHubUploadArtifactFileHttpResponse(
    long ContainerId,
    string ScopeIdentifier,
    string Path,
    string ItemType,
    string Status,
    long FileLength,
    int FileEncoding,
    int FileType,
    DateTime DateCreated,
    DateTime DateLastModified,
    string CreatedBy,
    string LastModifiedBy,
    long FileId,
    string ContentId);
#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via JSON generic type deserialization.

internal sealed class UploadArtifactFileHttpResponseValidator : AbstractValidator<GitHubUploadArtifactFileHttpResponse>
{
    public UploadArtifactFileHttpResponseValidator()
    {
        RuleFor(x => x.FileLength)
            .Must(fileLength => fileLength > 0)
            .WithMessage(x => $"$.fileLength must be a positive value. Actual value: '{x.FileLength.ToString(CultureInfo.InvariantCulture)}'.");
    }
}
