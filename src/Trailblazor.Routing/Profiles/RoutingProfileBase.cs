namespace Trailblazor.Routing.Profiles;

/// <summary>
/// Base class for routing profiles.
/// </summary>
public abstract class RoutingProfileBase : IRoutingProfile
{
    /// <summary>
    /// Method internally composes a <see cref="RoutingProfileConfiguration"/> from the profile.
    /// </summary>
    /// <returns>Composed <see cref="RoutingProfileConfiguration"/>.</returns>
    public RoutingProfileConfiguration ComposeConfigurationInternal()
    {
        var configuration = RoutingProfileConfiguration.Create();
        Configure(configuration);

        return configuration;
    }

    /// <summary>
    /// Method configures the given <paramref name="configuration"/>.
    /// </summary>
    /// <param name="configuration">Profile configuration to be configured.</param>
    protected abstract void Configure(RoutingProfileConfiguration configuration);
}
