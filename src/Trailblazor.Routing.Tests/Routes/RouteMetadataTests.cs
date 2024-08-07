using Trailblazor.Routing.Routes;
using Trailblazor.Routing.Tests.Registration.Components;

namespace Trailblazor.Routing.Tests.Routes;

public class RouteMetadataTests
{
    private Route NewRoute => Route.Empty<DummyComponent>();

    [Fact]
    public void Route_SetMetadataValue()
    {
        var route = NewRoute;
        route.SetMetadataValue("value", 33);

        var metadata = route.GetMetadata();
        Assert.True(metadata.TryGetValue("value", out var value) && value is int intValue && intValue == 33);
    }

    [Fact]
    public void Route_GetMetadataValue_Found()
    {
        var route = NewRoute;
        route.SetMetadataValue("value", 33);

        var value = route.GetMetadataValue<int?>("value");
        Assert.Equal(value, 33);
    }

    [Fact]
    public void Route_GetMetadataValue_NotFound()
    {
        var route = NewRoute;

        var value = route.GetMetadataValue<int?>("value");
        Assert.Null(value);
    }

    [Fact]
    public void Route_GetMetadataValue_NotFoundDefaultValue()
    {
        var now = DateTime.Now;
        var route = NewRoute;

        var value = route.GetMetadataValue<DateTime?>("value", now);
        Assert.Equal(value, now);
    }

    [Fact]
    public void Route_SetMetadataValue_OverrideExisting()
    {
        var route = NewRoute;

        route.SetMetadataValue("value", 33);
        route.SetMetadataValue("value", 56);

        var value = route.GetMetadataValue<int?>("value");
        Assert.Equal(value, 56);
    }

    [Fact]
    public void Route_HasMetadataValue()
    {
        var route = NewRoute;
        route.SetMetadataValue("value", 33);

        var hasValue = route.HasMetadataValue("value");
        Assert.True(hasValue);
    }

    [Fact]
    public void Route_RemoveMetadata()
    {
        var route = NewRoute;
        route.SetMetadataValue("value", 33);
        route.RemoveMetadataValue("value");

        var metadata = route.GetMetadata();
        Assert.Empty(metadata);
    }
}
