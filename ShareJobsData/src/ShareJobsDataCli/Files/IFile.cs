namespace ShareJobsDataCli.Files;

public interface IFile
{
    Task WriteAllTextAsync(string filename, string text);

    StreamWriter CreateFileStreamWriter(string filename);
}
