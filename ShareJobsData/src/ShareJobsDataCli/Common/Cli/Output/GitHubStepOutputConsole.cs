namespace ShareJobsDataCli.Common.Cli.Output;

// Couldn't find another way to change the output stream for without creating
// a new implementation of IConsole based on the default SystemConsole.
// How to write to the GitHub step's output was based on this comment https://github.com/community/community/discussions/35994#discussioncomment-4153598
public class GitHubStepOutputConsole : IConsole, IDisposable
{
    private readonly SystemConsole _systemConsole;
    private StreamWriter? _textWriter;

    public GitHubStepOutputConsole()
    {
        _systemConsole = new SystemConsole();
        Input = _systemConsole.Input;
        Error = _systemConsole.Error;
        Output = SetOutput();
    }

    public ConsoleReader Input { get; }

    public bool IsInputRedirected => _systemConsole.IsInputRedirected;

    public ConsoleWriter Output { get; }

    public bool IsOutputRedirected => _systemConsole.IsOutputRedirected;

    public ConsoleWriter Error { get; }

    public bool IsErrorRedirected => _systemConsole.IsErrorRedirected;

    public ConsoleColor ForegroundColor
    {
        get => _systemConsole.ForegroundColor;
        set => _systemConsole.ForegroundColor = value;
    }

    public ConsoleColor BackgroundColor
    {
        get => _systemConsole.BackgroundColor;
        set => _systemConsole.BackgroundColor = value;
    }

    public int WindowWidth
    {
        get => _systemConsole.WindowWidth;
        set => _systemConsole.WindowWidth = value;
    }

    public int WindowHeight
    {
        get => _systemConsole.WindowHeight;
        set => _systemConsole.WindowHeight = value;
    }

    public int CursorLeft
    {
        get => _systemConsole.CursorLeft;
        set => _systemConsole.CursorLeft = value;
    }

    public int CursorTop
    {
        get => _systemConsole.CursorTop;
        set => _systemConsole.CursorTop = value;
    }

    public void Clear() => _systemConsole.Clear();

    public ConsoleKeyInfo ReadKey(bool intercept = false) => _systemConsole.ReadKey(intercept);

    public CancellationToken RegisterCancellationHandler() => _systemConsole.RegisterCancellationHandler();

    public void ResetColor() => _systemConsole.ResetColor();

    private ConsoleWriter SetOutput()
    {
        // create a ConsoleWriter based on the example from https://github.com/Tyrrrz/CliFx/blob/02dc7de12721eacc729aaf50297b74b8a1c92ac0/CliFx/Infrastructure/ConsoleWriter.cs#L275
        // except that this writer will write to the GitHub step output file
        var githubOutputFile = Environment.GetEnvironmentVariable("GITHUB_OUTPUT") ?? string.Empty;
        _textWriter = new StreamWriter(githubOutputFile, append: true, Encoding.UTF8);
        return new ConsoleWriter(this, Stream.Synchronized(_textWriter.BaseStream))
        {
            AutoFlush = true,
        };
    }

    public void Dispose()
    {
        _systemConsole.Dispose();
        _textWriter?.Dispose();
        Output.Dispose();
    }
}
