namespace ShareJobsDataCli.Common;

// Using part of the idea of a ThrowHelper pattern due to the benefits explained here: https://dunnhq.com/posts/2022/throw-helper/
// See also: https://github.com/dotnet/runtime/blob/953f52482ac2460e4b3faff33e4f73c9b30cd7b4/src/libraries/System.Memory/src/System/ThrowHelper.cs
//
// Can't throw the exception from the methods because I don't use this exception for validation purposes at the start of methods,
// I use it to short circuit execution flow.

public sealed class UnexpectedTypeException : Exception
{
    internal UnexpectedTypeException(string message)
        : base(message)
    {
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static UnexpectedTypeException Create(object value, [CallerArgumentExpression("value")] string expression = "")
    {
        var type = value.GetType();
        var message = $"{expression} represented an unhandled type: '{type.Name}' in '{type.Namespace}'.";
        return new UnexpectedTypeException(message);
    }
}
