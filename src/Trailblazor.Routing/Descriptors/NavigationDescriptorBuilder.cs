using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Trailblazor.Routing.Exceptions;

namespace Trailblazor.Routing.Descriptors;

/// <summary>
/// Builder for generic navigation descriptors.
/// </summary>
/// <typeparam name="TComponent">Type of component to be navigated to.</typeparam>
public class NavigationDescriptorBuilder<TComponent>
    where TComponent : class, IComponent
{
    private readonly NavigationDescriptor _navigationDescriptor = new();

    internal NavigationDescriptorBuilder() { }

    /// <summary>
    /// Method adds a query parameter to the descriptors <see cref="Uri"/>.
    /// </summary>
    /// <typeparam name="TParameter">Type of the parameter value.</typeparam>
    /// <param name="paramterExpression">Expression expressing what parameter is to be addressed.</param>
    /// <param name="parameterValue">Value of the query parameter.</param>
    /// <returns>Navigation descriptor for further navigation configuration.</returns>
    public NavigationDescriptorBuilder<TComponent> WithParameter<TParameter>(Expression<Func<TComponent, TParameter>> paramterExpression, [DisallowNull] TParameter parameterValue)
    {
        ArgumentNullException.ThrowIfNull(parameterValue, nameof(parameterValue));
        MemberInfo? memberInfo = null;

        if (paramterExpression.Body is MemberExpression memberExpression)
            memberInfo = memberExpression.Member;
        else if (paramterExpression.Body is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression unaryMemberExpression)
            memberInfo = unaryMemberExpression.Member;

        if (memberInfo == null)
            throw new ArgumentException("The expression is not a member access expression.", nameof(paramterExpression));

        var parameterPropertyAttributes = memberInfo.GetCustomAttributes(true);
        var queryParameterAttribute = parameterPropertyAttributes.SingleOrDefault(p => p.GetType() == typeof(QueryParameterAttribute)) as QueryParameterAttribute
            ?? throw new MemberNotAQueryParameterException(memberInfo.Name);
        var parameterAttribute = parameterPropertyAttributes.SingleOrDefault(p => p.GetType() == typeof(ParameterAttribute)) as ParameterAttribute
            ?? throw new MemberNotAParameterException(memberInfo.Name);

        var queryParameterValue = parameterValue.ToString();
        if (queryParameterValue != null)
            _navigationDescriptor.QueryParameters.Add(queryParameterAttribute.Name ?? memberInfo.Name, queryParameterValue);

        return this;
    }

    /// <summary>
    /// Method adds a query parameter to the descriptors <see cref="Uri"/>.
    /// </summary>
    /// <param name="queryParameterName">Name of the query parameter.</param>
    /// <param name="queryParameterValue">Value of the query parameter.</param>
    /// <returns>Navigation descriptor for further navigation configuration.</returns>
    public NavigationDescriptorBuilder<TComponent> AddParameter(string queryParameterName, [DisallowNull] object queryParameterValue)
    {
        ArgumentNullException.ThrowIfNull(queryParameterValue, nameof(queryParameterValue));

        _navigationDescriptor.QueryParameters.Add(queryParameterName, queryParameterValue.ToString()!);
        return this;
    }

    /// <summary>
    /// Method sets the URI that is to be navigated to.
    /// </summary>
    /// <param name="uri">URI to be navigated to.</param>
    /// <returns>Navigation descriptor for further navigation configuration.</returns>
    public NavigationDescriptorBuilder<TComponent> WithUri([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
    {
        _navigationDescriptor.Uri = uri.TrimStart('/');
        return this;
    }

    /// <summary>
    /// Method sets the <see cref="NavigationOptions"/> that are to be used when navigating.
    /// </summary>
    /// <param name="options"><see cref="NavigationOptions"/> for configuring navigation details.</param>
    /// <returns>Navigation descriptor for further navigation configuration.</returns>
    public NavigationDescriptorBuilder<TComponent> WithOptions(NavigationOptions options)
    {
        _navigationDescriptor.Options = options;
        return this;
    }

    /// <summary>
    /// Method returns the configured navigation descriptor.
    /// </summary>
    /// <returns>Configured navigation descriptor</returns>
    internal NavigationDescriptor Build()
    {
        return _navigationDescriptor;
    }
}
