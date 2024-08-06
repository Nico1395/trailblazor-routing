using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Reflection;
using Trailblazor.Routing.DependencyInjection;
using Trailblazor.Routing.Extensions;

namespace Trailblazor.Routing;

/// <summary>
/// Service parses query parameter values from strings into their respective type.
/// </summary>
internal sealed class QueryParameterParser(IRoutingOptionsAccessor _routingOptionsProvider) : IQueryParameterParser
{
    private CultureInfo? _numericParseCultureInfo;
    private CultureInfo? _dateTimeParseCultureInfo;

    private CultureInfo NumericParseCultureInfo => _numericParseCultureInfo ??= _routingOptionsProvider.GetRoutingOptions().QueryParameterParseOptions.NumericParseCultureInfo();
    private CultureInfo DateTimeParseCultureInfo => _dateTimeParseCultureInfo ??= _routingOptionsProvider.GetRoutingOptions().QueryParameterParseOptions.DateTimeParseCultureInfo();

    /// <summary>
    /// Method parses <paramref name="rawQueryParameters"/> for the query parameter properties for components of type <paramref name="componentType"/>.
    /// </summary>
    /// <param name="rawQueryParameters">Raw unparsed query parameters from the URI.</param>
    /// <param name="componentType">Type of component the <paramref name="rawQueryParameters"/> are to be parsed for.</param>
    /// <returns>Parsed component parameters.</returns>
    public Dictionary<string, object?> ParseToComponentParameters(Dictionary<string, string> rawQueryParameters, Type componentType)
    {
        var componentQueryParameterProperties = componentType
            .GetProperties()
            .Where(p =>
                p.GetCustomAttribute<QueryParameterAttribute>() != null &&
                p.GetCustomAttribute<ParameterAttribute>() != null)
            .ToArray();

        return rawQueryParameters.Select(rawQueryParameter =>
        {
            var queryParameterProperty = FindQueryParameterProperty(componentQueryParameterProperties, rawQueryParameter);
            if (queryParameterProperty == null)
                return new KeyValuePair<string, object?>(string.Empty, default);

            var propertyValue = ParseValueForProperty(rawQueryParameter.Value, queryParameterProperty.PropertyType);
            return new KeyValuePair<string, object?>(queryParameterProperty.Name, propertyValue);
        })
        .Where(p => p.Key != string.Empty)
        .ToDictionary();
    }

    /// <summary>
    /// Method finds the <see cref="PropertyInfo"/> of the query parameter property.
    /// </summary>
    /// <param name="componentQueryParameterProperties">Components query parameter properties.</param>
    /// <param name="uriQueryParameter">Query parameters from the URI as a key-value-pair.</param>
    /// <returns><see cref="PropertyInfo"/> of the query parameter proeprty for the <paramref name="uriQueryParameter"/>, if found.</returns>
    private PropertyInfo? FindQueryParameterProperty(PropertyInfo[] componentQueryParameterProperties, KeyValuePair<string, string> uriQueryParameter)
    {
        return componentQueryParameterProperties.SingleOrDefault(p =>
        {
            var queryParameterAttribute = p.GetCustomAttribute<QueryParameterAttribute>();
            if (queryParameterAttribute!.Name != null)
                return queryParameterAttribute.Name.Equals(uriQueryParameter.Key, StringComparison.CurrentCultureIgnoreCase);

            return p.Name.Equals(uriQueryParameter.Key, StringComparison.CurrentCultureIgnoreCase);
        });
    }

    /// <summary>
    /// Method parses the specified <paramref name="queryParameterValue"/> to a value of the <paramref name="componentParameterPropertyType"/>.
    /// </summary>
    /// <param name="queryParameterValue">Query parameter value.</param>
    /// <param name="componentParameterPropertyType">Type of the property the <paramref name="queryParameterValue"/> is to be parsed into.</param>
    /// <returns>Parsed component parameter property type.</returns>
    private object? ParseValueForProperty(string queryParameterValue, Type componentParameterPropertyType)
    {
        if (componentParameterPropertyType.IsString())
            return queryParameterValue;
        else if (componentParameterPropertyType.IsBool() && bool.TryParse(queryParameterValue, out var boolValue))
            return boolValue;
        else if (componentParameterPropertyType.IsGuid() && Guid.TryParse(queryParameterValue, out var guidValue))
            return guidValue;
        else if (componentParameterPropertyType.IsTimeOnly() && TimeOnly.TryParse(queryParameterValue, DateTimeParseCultureInfo, out var timeOnlyValue))
            return timeOnlyValue;
        else if (componentParameterPropertyType.IsDateOnly() && DateOnly.TryParse(queryParameterValue, DateTimeParseCultureInfo, out var dateOnlyValue))
            return dateOnlyValue;
        else if (componentParameterPropertyType.IsDateTime() && DateTime.TryParse(queryParameterValue, DateTimeParseCultureInfo, out var dateTimeValue))
            return dateTimeValue;
        else if (componentParameterPropertyType.IsInt() && int.TryParse(queryParameterValue, NumericParseCultureInfo, out var intValue))
            return intValue;
        else if (componentParameterPropertyType.IsDouble() && double.TryParse(queryParameterValue, NumericParseCultureInfo, out var doubleValue))
            return doubleValue;
        else if (componentParameterPropertyType.IsLong() && long.TryParse(queryParameterValue, NumericParseCultureInfo, out var longValue))
            return longValue;
        else if (componentParameterPropertyType.IsDecimal() && decimal.TryParse(queryParameterValue, NumericParseCultureInfo, out var decimalValue))
            return decimalValue;

        return null;
    }
}
