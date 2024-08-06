using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using Trailblazor.Routing.Descriptors;

namespace Trailblazor.Routing;

/// <summary>
/// Service provides navigation functionalities.
/// </summary>
public interface INavigator
{
    /// <summary>
    /// Method navigates to the specified <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">URI to navigate to.</param>
    /// <param name="forceRefresh">Determines whether to force a new HTTP request when navigating or not.</param>
    /// <param name="replaceHistoryStackEntry">Determines whether to replace to current history stack entry or not.</param>
    public void NavigateTo([StringSyntax(StringSyntaxAttribute.Uri)] string uri, bool forceRefresh = false, bool replaceHistoryStackEntry = false);

    /// <summary>
    /// Method navigates to the specified <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">URI to navigate to.</param>
    /// <param name="navigationOptions">Options for describing how to navigate to the <paramref name="uri"/>.</param>
    public void NavigateTo([StringSyntax(StringSyntaxAttribute.Uri)] string uri, NavigationOptions navigationOptions);

    /// <summary>
    /// Method navigates to the URI resulting from the <see cref="NavigationDescriptor"/> configured using the <paramref name="descriptorAction"/>.
    /// </summary>
    /// <param name="descriptorAction">Action for configuring a <see cref="NavigationDescriptor"/>.</param>
    public void NavigateTo(Action<NavigationDescriptor> descriptorAction);

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
        where TComponent : IComponent;
}
