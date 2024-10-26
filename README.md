# Trailblazor Routing
**Trailblazor Routing** is a flexible and alternative Approach to routing and navigation for Blazor applications.

By default routes are scanned for and registered through `@page` directives at the top of components. While thats still possible, **Trailblazor Routing** allows setting up `RoutingProfiles` to configure routes and their metadata.

## Blazor Support
Generally all forms of Blazor are supported. WebAssembly and MAUI Hybrid work without any hick-ups.

_**As of right now**_ Blazor Server apps, and thus Blazor Web Apps using a Server and WASM hybrid constellation, are required to create a dummy component that have an `@page "/"` directive placed in the server side project. For the moment I dont really know why this is required but otherwise the browser doesnt find the website if no landing page is present.

However dont sweat, that component is not used as long as you have another component configured to use the landing page URI (so `"/"`).

## Dependency Injection
1. Use the extension method `.AddTrailblazorRouting(Action<RoutingOptions>? options = null)` to the `IServiceCollection` in your startup of your application.
2. Use the options action parameter to configure your routing and navigation specifics.

```cs
builder.Services.AddTrailblazorRouting(options => 
{
    // Register using assemblies and '@page' directives
    options.ScanForComponentsInAssemblies(typeof(Program).Assembly);

    // Register using assemblies
    options.AddProfilesFromAssemblies(typeof(Program).Assembly);

    // Register profiles explicitly
    options.AddProfile<RoutingProfile>();
});
```

### @page Directive Support
The support for `@page` directives is limited. But any component with that directive can be scanned for and registered as a route.

#### Limitations
- Query parameters have to be configured using the `ParameterAttribute` and either `SupplyParameterFromQueryAttribute` or `QueryParameterAttribute`.
- Following syntax is _**not**_ supported: `@page "/some-uri/{SomeParameterProperty}`
- _**To be clear:**_ Query parameters in the URI of `@page` directives are explicitly now supported as of now!

For specifics about query parameters see the appropriate section below.

### Blazor Web App Hybrid
When using a Blazor Web App Hybrid the dependecy injection for the router needs to be done server side. 

## Registration of Routes using `RoutingProfiles`
1. Write a class that inherits from the `RoutingProfileBase` class.
2. Implement the base classes' abstract members.
3. Either add the profile or its assembly to your `RoutingOptions` in your startup.
4. Add routes using the `RouteBuilder` the `RoutingProfileConfiguration` exposes.

```cs
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
```

## Managing Routes
Routes can the edited, deleted and overriden. These actions however are limited to the routes of their profile and inheriting and inherited profiles.

```cs
configuration.EditRoute<Home>("/", r => r.WithMetadataValue("title", "Home"));

configuration.OverrideRoute<DummyCounter, Counter>("/counter", r => r.WithMetadataValue("custom-metadata", "wow"));

configuration.AddRoute<DummyCounter>(r => r.WithUri("/dummy-counter"));
configuration.RemoveRoute<DummyCounter>("/dummy-counter");
```

### Route Relationships
Routes can cascade, meaning have children and a parent. Of course this is completely optional and doesnt have any relevance for routing and navigation, however if one would want to write breadcrumb components or a cascading menu, this would make it much easier.

### Inheritance Of Profiles
The most derived profiles are being registered, while the inherited profiles are ignored when scanning assemblies. Duplicates are filtered out as well. This obviously needs to be done so no duplicate routes or multiple uses of routes end up in the router.

You'll know what to do if there are any problems regarding this.

## Component Query Parameters
Since components can be registered without routes, their query parameters cannot be integrated into their routes 'in-component' anymore (so in the URI of the `@page` directive).

However the `QueryParameterAttribute` and standard ASP.NET `SupplyParameterFromQueryAttribute` can be used to accept query parameters from the current URI of the application.

1. Add a property for a query parameter to your component.
2. Use the `QueryParameterAttribute` or `SupplyParameterFromQueryAttribute` and specify the query parameters name to the attribute.
3. Add the `ParameterAttribute` to the property. This is required since these are still components and the query parameters are dealed to the components from the outside using parameters. As a result these properties need to be public and have a public getter and setter.

### Important:
- Using the `SupplyParameterFromQueryAttribute` makes the application run into an (to my knowledge) uncatchable exception if the type of the query parameter value and the type of the component parameter property don't match up. As far as I know, a lot of the times the type of the query parameter value will simply be a string.
- Since the `QueryParameterAttribute` is written and evaluated by **Trailblazor Routing**, query parameters that dont match the type of the components parameter property equivalent are simply ignored. Query parameter values are always attempted to be parsed into the component properties type, meaning you can specify any kind of type that one would parse into a query parameter. This makes query parameters in components much much easier and would be my preferred choice.

```cs
<PageTitle>Counter</PageTitle>

<h1>Counter</h1>

<p role="status">Current count: @_counter</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code
{
    private int _counter = 0;

    [Parameter]
    [QueryParameter("differentParameterName")]
    public int? InitialCounter { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (InitialCounter.HasValue)
            _counter = InitialCounter.Value;
    }

    private void IncrementCount()
    {
        _counter++;
    }
}

```

## Navigating Using Components
An alternative to navigation is to navigate using components. This is especially useful when navigating to components with query parameters. However this approach is obviously not mandatory.

For navigation using **Trailblazor Routing** the `INavigator` interface can be injected and used. The interface also offers more customized navigation using configurable `NavigationDescriptor`s.

### Multiple URIs per component
If multiple routes, essentially meaning URIs are found for a component, it is unclear what URI to pick for navigation. Thats why, in this case, its mandatory to specify the URI to be navigated to.

```cs
public partial class Home : ComponentBase
{
    [Inject]
    private INavigator Navigator { get; set; } = null!;
    
    private void NavigateToCounter()
    {
        Navigator.NavigateTo<Counter>(d => d.WithParameter(c => c.InitialCounter, 50));
    }

    private void NavigateToSomeComponent()
    {
        // In case the component has two routes/URIs associated with it, the target URI has to be specified
        Navigator.NavigateTo<SomeComponent>(d => d
            .WithParameter(c => c.SomeQueryParameter, DateTime.Now)
            .WithUri("/second-uri"));
    }
}
```

## Accessing The Current `RouterContext`
The router passes down a `RouterContext` as a cascading value. That record contains the current URI, with and without query parameters, current query parameters, routes and route data.

Components can accept the `RouterContext` as a cascading parameter. Any class can access the current `RouterContext` using the `IRouterContextAccessor` interface.

```cs
[Inject]
private IRouterContextAccessor RouterContextAccessor { get; set; } = null!;

[CascadingParameter]
public required RouterContext RouterContext { get; set; }
```

## Accessing Registered Routes
Accessing registered routes can easily be done using the `IRouteProvider` interface.

```cs
public class Service(IRouteProvider _routeProvider)
{
    public void DoSomething()
    {
        var routes = _routeProvider.GetRoutes();
        var currentRoute = _routeProvider.GetCurrentRoute();
        var desiredRoute = _routeProvider.FindRoute("some-uri");
        var routesAssociatedWithComponent = _routeProvider.FindRoutes(typeof(SomeComponent));
        var isCurrentRoute = _routeProvider.IsCurrentRoute(route);
    }
}
```

## Accessing Routing Options

Accessing routing options can easily be done using the `IRoutingOptionsAccessor` interface. The `RoutingOptions` themselves are not registered to the `IServiceCollection` and thus cannot be injected.

```cs
public class Service(IRoutingOptionsAccessor _routingOptionsAccessor)
{
    private readonly RoutingOptions _routingOptions = _routingOptionsAccessor.GetRoutingOptions();
}
```
