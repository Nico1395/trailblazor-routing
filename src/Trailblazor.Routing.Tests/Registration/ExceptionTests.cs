using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing.DependencyInjection;
using Trailblazor.Routing.Exceptions;
using Trailblazor.Routing.Routes;
using Trailblazor.Routing.Tests.Registration.Components;
using Trailblazor.Routing.Tests.Registration.Profiles;

namespace Trailblazor.Routing.Tests.Registration;

public class ExceptionTests
{
    [Fact]
    public void AbstractComponentException_Throws()
    {
        var exceptionWasThrown = false;

        try
        {
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddCommonTestServices()
                .AddTrailblazorRouting(options =>
                {
                    options.AddRoute<ComponentBase>(r => r.WithUri("test/uri"));
                })
                .BuildServiceProvider();
        }
        catch (AbstractComponentException)
        {
            exceptionWasThrown = true;
        }

        Assert.True(exceptionWasThrown);
    }

    [Fact]
    public void UriRegisteredToMultipleRoutesException_Throws()
    {
        var exceptionWasThrown = false;

        try
        {
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddCommonTestServices()
                .AddTrailblazorRouting(options =>
                {
                    options.AddRoute<DummyComponent>(r => r.WithUri("test/uri"));
                    options.AddRoute<DummyComponent>(r => r.WithUri("test/uri"));
                })
                .BuildServiceProvider();

            var route = serviceProvider.GetRequiredService<IRouteProvider>().FindRoute("test/uri");
        }
        catch (Exception)
        {
            exceptionWasThrown = true;
        }

        Assert.True(exceptionWasThrown);
    }

    [Fact]
    public void AbstractRoutingProfileException_Throws()
    {
        var exceptionWasThrown = false;

        try
        {
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddCommonTestServices()
                .AddTrailblazorRouting(options =>
                {
                    options.AddProfile(typeof(AbstractRoutingProfile));
                })
                .BuildServiceProvider();

            var route = serviceProvider.GetRequiredService<IRouteProvider>().FindRoute("test/uri");
        }
        catch (AbstractRoutingProfileException)
        {
            exceptionWasThrown = true;
        }

        Assert.True(exceptionWasThrown);
    }

    [Fact]
    public void TypeIsNotARoutingProfileException_Throws()
    {
        var exceptionWasThrown = false;

        try
        {
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddCommonTestServices()
                .AddTrailblazorRouting(options =>
                {
                    options.AddProfile(typeof(List<Route>));
                })
                .BuildServiceProvider();

            var route = serviceProvider.GetRequiredService<IRouteProvider>().FindRoute("test/uri");
        }
        catch (TypeIsNotARoutingProfileException)
        {
            exceptionWasThrown = true;
        }

        Assert.True(exceptionWasThrown);
    }
}
