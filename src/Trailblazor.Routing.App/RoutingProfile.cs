using Trailblazor.Routing.App.Samples;
using Trailblazor.Routing.Profiles;

namespace Trailblazor.Routing.App;

internal sealed class RoutingProfile : RoutingProfileBase
{
    protected sealed override void Configure(RoutingProfileConfiguration configuration)
    {
        configuration.AddRoute<Home>(r => r
            .WithUri("/")
            .WithChild<Weather>(r => r
                .WithUri("/weather")
                .WithMetadataValue("permission", "wouldnt-you-like-to-know-wheather-boi"))
            .WithChild<Counter>(r => r
                .WithUri("/counter")
                .WithMetadataValue("title", "Counter")
                .WithMetadataValue("subtitle", "Wow a counter, how fancy")
                .WithMetadataValue("permission", "be-a-fancy-counter")));
    }
}
