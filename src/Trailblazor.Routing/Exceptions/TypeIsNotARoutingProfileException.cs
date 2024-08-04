using Trailblazor.Routing.Profiles;

namespace Trailblazor.Routing.Exceptions;

/// <summary>
/// Exception expresses that a given type is not implementing <see cref="IRoutingProfile"/> and is therefor not a routing profile.
/// </summary>
public sealed class TypeIsNotARoutingProfileException : Exception
{
    internal TypeIsNotARoutingProfileException(Type type)
        : base($"Type '{type.FullName}' is not a routing profile and thus cannot be registered as one.")
    {
    }
}
