# Trailblazor Routing
**Trailblazor Routing** is a flexible and alternative Approach to routing and navigation for Blazor applications.

By default routes are scanned for and registered through `@page` directives at the top of components. While thats still possible, **Trailblazor Routing** allows setting up `RoutingProfiles` to configure routes and their metadata.

## Overview
### 

## Basic Setup
### Adding Trailblazor Routing
1. Use the extension method `.AddTrailblazorRouting(Action<RoutingOptions>? options = null)` to the `IServiceCollection` in your startup of your application.
2. Use the options action parameter to configure your routing and navigation specifics.

```cs
builder.Services.AddTrailblazorRouting(options => 
{
    options.AddProfilesFromAssemblies(typeof(Program).Assembly);
    options.AddProfile<RoutingProfile>();
    options.QueryParameterParseOptions = new QueryParameterParseOptions()
    {
        DateTimeStyles = DateTimeStyles.AssumeUniversal,
    };
});
```

### Registration of Routes using `RoutingProfiles`
1. Write a class that inherits from the `RoutingProfileBase` class.
2. Implement the base classes' abstract members.
3. Either add the profile or its assembly to your `RoutingOptions` in your startup.

```cs
internal sealed class RoutingProfile : RoutingProfileBase
{
    protected sealed override void Configure(RoutingProfileConfiguration configuration)
    {
        configuration.AddRoute<Home>(r => r.WithUri("/"));
        configuration.AddRoute<Counter>(r => r.WithUri("/counter"));
        configuration.AddRoute<Weather>(r => r.WithUri("/weather"));
    }
}
```

### Component Query Parameters
Since components can be registered without routes in the components, their parameters cannot be integrated into their routes 'in-component' anymore.

However the `QueryParameterAttribute` and standard ASP.NET `SupplyParameterFromQueryAttribute` can be used to accept query parameters from the current URI of the application.

1. Add a property for a query parameter to your component.
2. Use the `QueryParameterAttribute` or `SupplyParameterFromQueryAttribute` and specify the query parameters name to the attribute.
3. Add the `ParameterAttribute` to the property. This is required since these are still components and the query parameters are dealed to the components from the outside using parameters. As a result these properties need to be public and have a public getter and setter.

#### Important:
- Using the `SupplyParameterFromQueryAttribute` makes the application run into an (to my knowledge) uncatchable exception if the type of the query parameter value and the type of the component parameter property don#t match up.
- Since the `QueryParameterAttribute` is written and evaluated by **Trailblazor Routing**, query parameters that dont match the type of the components parameter property equivalent are simply ignored. Query parameter values are always attempted to be parsed into the component properties type.

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

### Navigating Using Components
