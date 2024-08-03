using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Trailblazor.Routing.DependencyInjection;
using Trailblazor.Routing.Extensions;
using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing;

/// <summary>
/// Trailblazor Router component. Orchestrates routing using the configured <see cref="RoutingOptions"/> and router profiles.
/// </summary>
public class TrailblazorRouter : IComponent, IHandleAfterRender, IDisposable
{
    private RenderHandle _renderHandle;
    private bool _navigationInterceptionEnabled;
    private string? _location;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private INavigationInterception NavigationInterception { get; set; } = null!;

    [Inject]
    private IRouteProvider RouteProvider { get; set; } = null!;

    [Inject]
    private IRouteParser RouteParser { get; set; } = null!;

    /// <summary>
    /// Required render fragment that is being rendered if a route has been found for the current relative URI.
    /// </summary>
    [Parameter, EditorRequired]
    public required RenderFragment<RouteData> Found { get; set; }

    /// <summary>
    /// Required render fragment that is being rendered if no route has been found for the current relative URI.
    /// </summary>
    [Parameter, EditorRequired]
    public required RenderFragment NotFound { get; set; }

    /// <summary>
    /// Required type of the main layout component to be used. This type has to derive from the <see cref="LayoutComponentBase"/>.
    /// </summary>
    [Parameter, EditorRequired]
    public required Type LayoutType { get; set; }

    /// <summary>
    /// Method handles disposing off of the router.
    /// </summary>
    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }

    /// <summary>
    /// Attaches the component to a <see cref="RenderHandle" />.
    /// </summary>
    /// <param name="renderHandle">A <see cref="RenderHandle"/> that allows the component to be rendered.</param>
    public void Attach(RenderHandle renderHandle)
    {
        _renderHandle = renderHandle;
        _location = NavigationManager.GetRelativeUri();

        NavigationManager.LocationChanged += OnLocationChanged;
    }

    /// <summary>
    /// Sets parameters supplied by the component's parent in the render tree.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <returns>A <see cref="Task"/> that completes when the component has finished updating and rendering itself.</returns>
    /// <remarks>
    /// The <see cref="SetParametersAsync(ParameterView)"/> method should be passed the entire set of parameter values each
    /// time <see cref="SetParametersAsync(ParameterView)"/> is called. It not required that the caller supply a parameter
    /// value for all parameters that are logically understood by the component.
    /// </remarks>
    public Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        if (!LayoutType.IsAssignableTo(typeof(LayoutComponentBase)))
            throw new InvalidOperationException($"The specified {nameof(LayoutType)} is not of derived of type '{typeof(LayoutComponentBase)}'.");

        InitiateRender();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Notifies the component that it has been rendered.
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous event handling operation.</returns>
    public async Task OnAfterRenderAsync()
    {
        if (!_navigationInterceptionEnabled)
        {
            _navigationInterceptionEnabled = true;
            await NavigationInterception.EnableNavigationInterceptionAsync();
        }
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs args)
    {
        _location = args.Location;
        InitiateRender();
    }

    private void InitiateRender()
    {
        if (_location == null)
            return;

        var relativeUri = NavigationManager.GetRelativeUri();
        var routerContext = RouterContext.Create(
            relativeUri,
            RouteParser.ParseQueryParameters(relativeUri),
            RouteProvider.GetCurrentRoute());

        Render(routerContext);
    }

    private void Render(RouterContext routerContext)
    {
        _renderHandle.Render(builder =>
        {
            builder.OpenComponent<CascadingValue<RouterContext>>(0);
            builder.AddComponentParameter(1, nameof(CascadingValue<RouterContext>.Value), routerContext);
            builder.AddComponentParameter(2, nameof(CascadingValue<RouterContext>.ChildContent), (RenderFragment)(content =>
            {
                content.AddContent(2, routerContext.Route != null ? Found(routerContext.RouteData) : NotFound);
            }));
            builder.CloseComponent();
        });
    }
}