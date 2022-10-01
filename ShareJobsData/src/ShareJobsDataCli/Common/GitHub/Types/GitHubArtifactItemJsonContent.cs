namespace ShareJobsDataCli.Common.GitHub.Types;

internal sealed record GitHubArtifactItemJsonContent
{
    private readonly JObject _value;

    private GitHubArtifactItemJsonContent(JObject jsonContent)
    {
        _value = jsonContent.NotNull();
    }

    public static CreateGitHubArtifactItemJsonContentResult Create(string content)
    {
        JObject jObject;
        try
        {
            jObject = JObject.Parse(content);
        }
        catch (JsonReaderException jsonReaderException)
        {
            return new GitHubArtifactItemNotJsonContent(content, jsonReaderException.Message);
        }

        return new GitHubArtifactItemJsonContent(jObject);
    }

    public JObject AsJObject()
    {
        return _value;
    }
}

internal abstract record CreateGitHubArtifactItemJsonContentResult
{
    private CreateGitHubArtifactItemJsonContentResult()
    {
    }

    public sealed record Ok(GitHubArtifactItemJsonContent JsonContent)
        : CreateGitHubArtifactItemJsonContentResult;

    public sealed record ArtifactItemNotJsonContent(GitHubArtifactItemNotJsonContent NotJsonContent)
        : CreateGitHubArtifactItemJsonContentResult;

    public static implicit operator CreateGitHubArtifactItemJsonContentResult(GitHubArtifactItemJsonContent jsonContent) => new Ok(jsonContent);

    public static implicit operator CreateGitHubArtifactItemJsonContentResult(GitHubArtifactItemNotJsonContent notJsonContent) => new ArtifactItemNotJsonContent(notJsonContent);

    public bool IsOk(
       [NotNullWhen(returnValue: true)] out GitHubArtifactItemJsonContent? jsonContent,
       [NotNullWhen(returnValue: false)] out GitHubArtifactItemNotJsonContent? notJsonContent)
    {
        jsonContent = null;
        notJsonContent = null;

        if (this is Ok ok)
        {
            jsonContent = ok.JsonContent;
            return true;
        }

        var invalidJsonError = (ArtifactItemNotJsonContent)this;
        notJsonContent = invalidJsonError.NotJsonContent;
        return false;
    }
}
