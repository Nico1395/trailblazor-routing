using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Trailblazor.Routing.Exceptions;
using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing.Descriptors;

/// <summary>
/// Navigation descriptor descrining a resulting URI.
/// </summary>
public sealed record NavigationDescriptor : NavigationDescriptorBase
{
    internal NavigationDescriptor() : base() { }
}

/// <summary>
/// Navigation descriptor descrining a resulting URI.
/// </summary>
/// <typeparam name="TComponent">Type of component associated with that URI through a registered <see cref="Route"/>.</typeparam>
public sealed record NavigationDescriptor<TComponent> : NavigationDescriptorBase
    where TComponent : IComponent
{
    internal NavigationDescriptor() : base() { }

    /// <summary>
    /// Method adds a query parameter to the descriptors <see cref="Uri"/>.
    /// </summary>
    /// <typeparam name="TParameter">Type of the parameter value.</typeparam>
    /// <param name="paramterExpression">Expression expressing what parameter is to be addressed.</param>
    /// <param name="parameterValue">Value of the query parameter.</param>
    /// <returns>Navigation descriptor for further configurations.</returns>
    public NavigationDescriptorBase AddParameter<TParameter>(Expression<Func<TComponent, TParameter>> paramterExpression, [DisallowNull] TParameter parameterValue)
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
            QueryParameters.Add(queryParameterAttribute.Name ?? memberInfo.Name, queryParameterValue);

        return this;
    }
}
