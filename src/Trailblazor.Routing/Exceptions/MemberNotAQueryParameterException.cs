namespace Trailblazor.Routing.Exceptions;

/// <summary>
/// Exception expressing that a member is not a component query parameter.
/// </summary>
public sealed class MemberNotAQueryParameterException : Exception
{
    internal MemberNotAQueryParameterException(string memberName)
        : base($"The member '{memberName}' does not have a {nameof(QueryParameterAttribute)} and thus cannot be used as a query parameter when navigating.")
    {
    }
}
