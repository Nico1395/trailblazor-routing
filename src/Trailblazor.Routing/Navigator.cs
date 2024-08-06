using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using Trailblazor.Routing.Descriptors;
using Trailblazor.Routing.Exceptions;

namespace Trailblazor.Routing;

internal sealed class Navigator(
    NavigationManager _navigationManager,
    IRouteProvider _routeProvider,
    IRouteParser _routeParser) : INavigator
{
    /// <summary>
    /// Method navigates to the specified <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">URI to navigate to.</param>
    /// <param name="forceRefresh">Determines whether to force a new HTTP request when navigating or not.</param>
    /// <param name="replaceHistoryStackEntry">Determines whether to replace to current history stack entry or not.</param>
    public void NavigateTo([StringSyntax(StringSyntaxAttribute.Uri)] string uri, bool forceRefresh = false, bool replaceHistoryStackEntry = false)
    {
        NavigateToInternal(new NavigationDescriptor()
        {
            Uri = uri,
            Options = new NavigationOptions()
            {
                ForceLoad = forceRefresh,
                ReplaceHistoryEntry = replaceHistoryStackEntry,
            }
        });
    }

    /// <summary>
    /// Method navigates to the specified <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">URI to navigate to.</param>
    /// <param name="navigationOptions">Options for describing how to navigate to the <paramref name="uri"/>.</param>
    public void NavigateTo([StringSyntax(StringSyntaxAttribute.Uri)] string uri, NavigationOptions navigationOptions)
    {
        NavigateToInternal(new NavigationDescriptor()
        {
            Uri = uri,
            Options = navigationOptions,
        });
    }

    /// <summary>
    /// Method navigates to the URI resulting from the <see cref="NavigationDescriptor"/> configured using the <paramref name="descriptorAction"/>.
    /// </summary>
    /// <param name="descriptorAction">Action for configuring a <see cref="NavigationDescriptor"/>.</param>
    public void NavigateTo(Action<NavigationDescriptor> descriptorAction)
    {
        var navigationDescriptor = new NavigationDescriptor();
        descriptorAction.Invoke(navigationDescriptor);

        NavigateToInternal(navigationDescriptor);
    }

    /// <summary>
    /// Method navigates to the component of type <typeparamref name="TComponent"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <typeparamref name="TComponent"/> has to be associated and registered with a route. If multiple routes are found an exception is being thrown
    /// since its impossible to know what route to navigate to, if there are multiple options. If that is the case consider configuring the URI of the
    /// <see cref="NavigationDescriptor{TComponent}"/> using the action.
    /// </para>
    /// <para>
    /// Allows setting query parameters by specifying properties and their values.
    /// </para>
    /// </remarks>
    /// <typeparam name="TComponent">Type of component associated with a route.</typeparam>
    /// <param name="descriptorAction">Action for configuring a <see cref="NavigationDescriptor{TComponent}"/>.</param>
    public void NavigateTo<TComponent>(Action<NavigationDescriptor<TComponent>>? descriptorAction = null)
        where TComponent : IComponent
    {
        var navigationDescriptor = new NavigationDescriptor<TComponent>();
        descriptorAction?.Invoke(navigationDescriptor);

        if (navigationDescriptor.Uri == null)
        {
            var componentType = typeof(TComponent);
            var targetRoutes = _routeProvider.FindRoutes(componentType);

            if (targetRoutes.Count != 1)
                throw new MultipleUrisFoundForComponentException(componentType, targetRoutes.Count);

            navigationDescriptor.Uri = targetRoutes[0].Uri;
        }

        NavigateToInternal(navigationDescriptor);
    }

    /// <summary>
    /// Method navigates to the URI the specified <paramref name="navigationDescriptor"/> results in.
    /// </summary>
    /// <param name="navigationDescriptor">Navigation descriptor describing the route to navigate to.</param>
    private void NavigateToInternal(NavigationDescriptorBase navigationDescriptor)
    {
        if (navigationDescriptor.Uri == null)
            throw new NoUriSpecifiedException();

        var uriWithQueryParameters = _routeParser.AddQueryParameters(navigationDescriptor.Uri, navigationDescriptor.QueryParameters);

        _navigationManager.NavigateTo(
            uriWithQueryParameters,
            navigationDescriptor.Options);
    }
}
