namespace Trailblazor.Routing;

/// <summary>
/// Service parses query parameter values from strings into their respective type.
/// </summary>
internal interface IQueryParameterParser
{
    /// <summary>
    /// Method attempts to parse the specified <paramref name="stringValue"/>. If the string could not be parsed into
    /// any primitive type or standard struct such as <see cref="Guid"/> or <see cref="DateTime"/>, then the specified
    /// <paramref name="stringValue"/> will be returned.
    /// </summary>
    /// <param name="stringValue">Query parameter value as a string to be parsed.</param>
    /// <returns>Parsed <paramref name="stringValue"/>.</returns>
    internal object ParseValue(string stringValue);
}
