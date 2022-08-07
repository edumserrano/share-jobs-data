namespace ShareJobsDataCli.GitHub.UploadArtifact.HttpModels.Responses;

internal sealed record GitHubListArtifactsResponse
(
    List<GitHubArtifactFileContainerResponse> Artifacts
);

internal sealed class GitHubListArtifactsResponseValidator : AbstractValidator<GitHubListArtifactsResponse>
{
    public GitHubListArtifactsResponseValidator()
    {
        RuleFor(x => x.Artifacts)
            .Must(x => x is not null)
            .WithMessage(x => $"{nameof(x.Artifacts)} cannot be null.");
        RuleFor(x => x.Artifacts)
            .Must(AllArtifactsMustBeValid)
            .WithMessage(x => $"{nameof(x.Artifacts)} contains invalid elements.");
    }

    private bool AllArtifactsMustBeValid(GitHubListArtifactsResponse instance, List<GitHubArtifactFileContainerResponse> artifacts, ValidationContext<GitHubListArtifactsResponse> context)
    {
        var hasErrors = false;
        var validator = new GitHubArtifactFileContainerResponseValidator();
        for (var i = 0; i < artifacts.Count; i++)
        {
            var artifact = artifacts[i];
            var validationResult = validator.Validate(artifact);
            if (!validationResult.IsValid)
            {
                hasErrors = true;
                foreach (var error in validationResult.Errors)
                {
                    error.ErrorMessage = $"Element {i} is invalid. {error.ErrorMessage}"; // update error message to "group" errors by failed artifact model instance
                    context.AddFailure(error);
                }
            }
        }

        return hasErrors;
    }
}
