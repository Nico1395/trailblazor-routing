using Microsoft.AspNetCore.Components;

namespace Trailblazor.Routing.Exceptions;

/// <summary>
/// Exception expressing that a member is not a component parameter.
/// </summary>
public sealed class MemberNotAParameterException : Exception
{
    internal MemberNotAParameterException(string memberName)
        : base($"The member '{memberName}' does have a {nameof(QueryParameterAttribute)} but not a '{nameof(ParameterAttribute)}' and thus cannot receive query parameter values.")
    {
    }
}
