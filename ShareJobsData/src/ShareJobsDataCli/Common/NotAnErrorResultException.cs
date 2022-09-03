namespace ShareJobsDataCli.Common;

// Using part of the idea of a ThrowHelper pattern due to the benefits explained here: https://dunnhq.com/posts/2022/throw-helper/
// See also: https://github.com/dotnet/runtime/blob/953f52482ac2460e4b3faff33e4f73c9b30cd7b4/src/libraries/System.Memory/src/System/ThrowHelper.cs

public sealed class NotAnErrorResultException : Exception
{
    internal NotAnErrorResultException(string message)
        : base(message)
    {
    }

    [DoesNotReturn]
    internal static void Throw(object result) => throw CreateNotAnErrorResultException(result);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static NotAnErrorResultException CreateNotAnErrorResultException(object result, [CallerArgumentExpression("result")] string expression = "")
    {
        var type = result.GetType();
        var message = $"{expression} should be a failed result but was {type.Name}.";
        return new NotAnErrorResultException(message);
    }
}
