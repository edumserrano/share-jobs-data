namespace ShareJobsDataCli;

public class GitHubStepOutputConsole : IConsole, IDisposable
{
    private readonly SystemConsole _systemConsole;
    private readonly StreamWriter _textWriter;

    public GitHubStepOutputConsole()
    {
        _systemConsole = new SystemConsole();
        Input = _systemConsole.Input;
        Error = _systemConsole.Error;

        var githubOutputFile = Environment.GetEnvironmentVariable("GITHUB_OUTPUT") ?? string.Empty;
        _textWriter = new StreamWriter(githubOutputFile, append: true, Encoding.UTF8);
        var consoleWriter = new ConsoleWriter(this, Stream.Synchronized(_textWriter.BaseStream))
        {
            AutoFlush = true,
        };
        Output = consoleWriter;
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

    public void Dispose()
    {
        _systemConsole.Dispose();
        _textWriter.Dispose();
        Output.Dispose();
    }
}
