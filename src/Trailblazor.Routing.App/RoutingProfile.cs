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
            .WithChild<DummyCounter>(r => r
                .WithUri("/counter")
                .WithMetadataValue("title", "Counter")
                .WithMetadataValue("subtitle", "Wow a counter, how fancy")
                .WithMetadataValue("permission", "be-a-fancy-counter")));

        configuration.EditRoute<Home>("/", r => r
            .WithMetadataValue("title", "Home"));

        configuration.OverrideRoute<DummyCounter, Counter>("/counter", r => r
            .WithMetadataValue("custom-metadata", "wow"));

        configuration.AddRoute<DummyCounter>(r => r.WithUri("/dummy-counter"));
        configuration.RemoveRoute<DummyCounter>("/dummy-counter");
    }
}
