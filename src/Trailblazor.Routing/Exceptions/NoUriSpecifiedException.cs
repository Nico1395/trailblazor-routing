namespace Trailblazor.Routing.Exceptions;

/// <summary>
/// Exception expresses that no URI has been specified when navigating.
/// </summary>
public sealed class NoUriSpecifiedException : Exception
{
    internal NoUriSpecifiedException()
        : base("No URI has been specified when attempting to navigate.")
    {
    }
}
