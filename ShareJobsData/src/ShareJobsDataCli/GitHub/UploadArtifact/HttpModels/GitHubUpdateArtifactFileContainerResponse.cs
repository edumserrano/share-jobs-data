namespace ShareJobsDataCli.GitHub.UploadArtifact.HttpModels;

internal sealed record GitHubUpdateArtifactFileContainerResponse
(
    int ContainerId,
    int Size,
    string FileContainerResourceUrl,
    string Type,
    string Name,
    string Url,
    DateTime ExpiresOn
);

internal sealed class GitHubUpdateArtifactFileContainerResponseValidator : AbstractValidator<GitHubUpdateArtifactFileContainerResponse>
{
    public GitHubUpdateArtifactFileContainerResponseValidator()
    {
        RuleFor(x => x.FileContainerResourceUrl)
            .Must(BeAValidUrl)
            .WithMessage(x => $"{nameof(x.FileContainerResourceUrl)} is not a valid URL. Actual value: '{x.FileContainerResourceUrl}'.");
    }

    private bool BeAValidUrl(string fileContainerResourceUrl)
    {
        var options = new UriCreationOptions();
        return Uri.TryCreate(fileContainerResourceUrl, options, out var _);
    }
}
