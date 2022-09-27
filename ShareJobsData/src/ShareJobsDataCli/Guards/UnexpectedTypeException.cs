namespace ShareJobsDataCli.Guards;

// Using part of the idea of a ThrowHelper pattern due to the benefits explained here: https://dunnhq.com/posts/2022/throw-helper/
// See also:
// https://github.com/dotnet/runtime/blob/9e81ca53137c587bce0f30bf60f13bb10fbdd204/src/libraries/System.Private.CoreLib/src/System/ArgumentNullException.cs#L63
// https://github.com/dotnet/runtime/blob/9e81ca53137c587bce0f30bf60f13bb10fbdd204/src/libraries/System.Private.CoreLib/src/System/ArgumentException.cs#L110
// https://github.com/dotnet/runtime/blob/953f52482ac2460e4b3faff33e4f73c9b30cd7b4/src/libraries/System.Memory/src/System/ThrowHelper.cs
//
// In short:
// "Having a throw new in your methods can be inefficient. The inefficiency comes from the fact that a fair amount of assembly code is
// generated to throw the exception."
// "... there is a pattern that can help both eliminate the extra assembly code instructions, and optimise the code so that registers
// are not set-up needlessly."
//
// Not using a "proper" ThrowHelper because I want to throw this exception from a switch case. If I had the ThrowHelper throw the exception
// the switch case doesn't know that and I still need to return something.
//
// The [MethodImpl(MethodImplOptions.NoInlining)] attribute for the 'Create' method is what avoids the extra exception setup assembly
// instructions from being populated on the call sites that use this method.
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
