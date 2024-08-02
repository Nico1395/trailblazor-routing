namespace Trailblazor.Routing.Profiles;

/// <summary>
/// Base class for routing profiles.
/// </summary>
internal interface IRoutingProfile
{
    /// <summary>
    /// Method internally composes a <see cref="RoutingProfileConfiguration"/> from the profile.
    /// </summary>
    /// <returns>Composed <see cref="RoutingProfileConfiguration"/>.</returns>
    internal RoutingProfileConfiguration ComposeConfigurationInternal();
}
