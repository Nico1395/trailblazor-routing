using Trailblazor.Routing.Profiles;

namespace Trailblazor.Routing.Exceptions;

/// <summary>
/// Exception expresses that a given type is not implementing <see cref="IRoutingProfile"/> and is therefor not a routing profile.
/// </summary>
public sealed class TypeIsNotAProfileException : Exception
{
    internal TypeIsNotAProfileException(Type type)
        : base($"Type '{type.FullName}' is not a routing profile and cannot be registered as one.")
    {
    }
}
