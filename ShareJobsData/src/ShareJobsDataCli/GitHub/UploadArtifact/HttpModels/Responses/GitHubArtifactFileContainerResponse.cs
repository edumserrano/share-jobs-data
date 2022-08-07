namespace ShareJobsDataCli.GitHub.UploadArtifact.HttpModels.Responses;

internal sealed record GitHubArtifactFileContainerResponse
(
    int ContainerId,
    int Size,
    string FileContainerResourceUrl,
    string Type,
    string Name,
    string Url,
    DateTime ExpiresOn
);

internal sealed class GitHubArtifactFileContainerResponseValidator : AbstractValidator<GitHubArtifactFileContainerResponse>
{
    public GitHubArtifactFileContainerResponseValidator()
    {
        RuleFor(x => x.FileContainerResourceUrl)
            .Must(BeAValidUrl)
            .WithMessage(x => $"{nameof(x.FileContainerResourceUrl)} is not a valid URL. Actual value: '{x.FileContainerResourceUrl}'.");
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(x => $"{nameof(x.Name)} must have a value. Actual value: '{x.FileContainerResourceUrl}'.");
    }

    private bool BeAValidUrl(string fileContainerResourceUrl)
    {
        var options = new UriCreationOptions();
        return Uri.TryCreate(fileContainerResourceUrl, options, out var _);
    }
}
