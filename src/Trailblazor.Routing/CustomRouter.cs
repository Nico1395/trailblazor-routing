using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Trailblazor.Routing;

public class CustomRouter : IRouter
{
    private RenderHandle _renderHandle;
    private bool _navigationInterceptionEnabled;
    private string? _location;

    [Inject]
    private INavigationService NavigationService { get; set; } = null!;

    [Inject]
    private INavigationInterception NavigationInterception { get; set; } = null!;

    [Inject]
    private IRouteProvider RouteProvider { get; set; } = null!;

    [Inject]
    private IRouteParser RouteParser { get; set; } = null!;

    [Parameter, EditorRequired]
    public required RenderFragment<RouteData> Found { get; set; }

    [Parameter, EditorRequired]
    public required RenderFragment NotFound { get; set; }

    [Parameter, EditorRequired]
    public required Type LayoutType { get; set; }

    public void Dispose()
    {
        NavigationService.UnsubscribeLocationChanged(OnLocationChanged);
    }

    public void Attach(RenderHandle renderHandle)
    {
        _renderHandle = renderHandle;
        _location = NavigationService.GetCurrentUri();

        NavigationService.SubscribeLocationChanged(OnLocationChanged);
    }

    public Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        if (!LayoutType.IsAssignableTo(typeof(LayoutComponentBase)))
            throw new InvalidOperationException($"The specified {nameof(LayoutType)} is not of derived of type '{typeof(LayoutComponentBase)}'.");

        InitiateRender();
        return Task.CompletedTask;
    }

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

        var relativeUri = NavigationService.GetCurrentRelativeUri();
        var route = RouteProvider.GetCurrentRoute();

        if (route != null)
            _renderHandle.Render(Found(new RouteData(route.Component, RouteParser.ParseQueryParameters(relativeUri))));
        else
            _renderHandle.Render(NotFound);
    }
}