using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using Trailblazor.Routing.DependencyInjection;
using Trailblazor.Routing.Exceptions;
using Trailblazor.Routing.Extensions;

namespace Trailblazor.Routing;

/// <summary>
/// Service parses query parameter values from strings into their respective type.
/// </summary>
internal sealed class ComponentParameterParser(
    IRoutingOptionsAccessor _routingOptionsProvider,
    IUriParser _uriParser) : IComponentParameterParser
{
    private CultureInfo? _numericParseCultureInfo;
    private CultureInfo? _dateTimeParseCultureInfo;

    private readonly string[] _validDirectiveParameterTypes =
    {
        "bool", "datetime", "decimal", "double", "float", "guid", "int", "long", "timeonly", "dateonly", "string"
    };

    private CultureInfo NumericParseCultureInfo => _numericParseCultureInfo ??= _routingOptionsProvider.GetRoutingOptions().QueryParameterParseOptions.NumericParseCultureInfo();
    private CultureInfo DateTimeParseCultureInfo => _dateTimeParseCultureInfo ??= _routingOptionsProvider.GetRoutingOptions().QueryParameterParseOptions.DateTimeParseCultureInfo();

    /// <summary>
    /// Method parses <paramref name="rawQueryParameters"/> for the query parameter properties for components of type <paramref name="componentType"/>.
    /// </summary>
    /// <param name="rawQueryParameters">Raw unparsed query parameters from the URI.</param>
    /// <param name="componentType">Type of component the <paramref name="rawQueryParameters"/> are to be parsed for.</param>
    /// <returns>Parsed component parameters.</returns>
    public Dictionary<string, object?> ParseFromQueryParameters(Dictionary<string, string> rawQueryParameters, Type componentType)
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

    public Dictionary<string, object?> ParseFromDirectiveQueryParameters(string relativeUri, Type componentType, string routeUri)
    {
        var routeUriSegments = _uriParser.ParseSegments(routeUri);
        var queryParameterDescriptors = routeUriSegments
            .Select(segment =>
            {
                var segmentPosition = Array.FindIndex(routeUriSegments, s => s == segment);
                var match = Regex.Match(segment, @"\{[^}]+\}");

                if (match.Success)
                {
                    var segmentParameter = match.Value.Trim('{', '}');
                    return (segmentPosition, segmentParameter);
                }

                return default;
            })
            .Where(s => s != default)
            .ToList();

        var relativeUriSegments = _uriParser.ParseSegments(relativeUri);
        var directiveQueryParmeters = new Dictionary<string, object?>();

        foreach (var queryParameterDescriptor in queryParameterDescriptors)
        {
            if (relativeUriSegments.Length - 1 < queryParameterDescriptor.segmentPosition)
                continue;

            var parameterValueSegment = relativeUriSegments[queryParameterDescriptor.segmentPosition];
            var parameterDescriptorSegmentArguments = queryParameterDescriptor.segmentParameter.Split(':');
            var parameterValue = GetDirectiveParameterValue(parameterDescriptorSegmentArguments, parameterValueSegment);

            if (parameterValue != null)
                directiveQueryParmeters.Add(parameterDescriptorSegmentArguments[0], parameterValue);
        }

        return directiveQueryParmeters;
    }

    private object? GetDirectiveParameterValue(string[] parameterDescriptorSegmentArguments, string parameterValueSegment)
    {
        if (parameterDescriptorSegmentArguments.Length == 1)
        {
            // TODO -> {*catchAllParameters}
            return parameterValueSegment;
        }
        else
        {
            var isValidType = _validDirectiveParameterTypes.Any(t => t.Equals(parameterDescriptorSegmentArguments[1].TrimEnd('?'), StringComparison.OrdinalIgnoreCase));
            if (!isValidType)
                throw new InvalidDirectiveQueryParameterTypeException();

            return parameterDescriptorSegmentArguments[1] switch
            {
                "bool" => bool.Parse(parameterValueSegment),
                "datetime" => DateTime.Parse(parameterValueSegment, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                "decimal" => decimal.Parse(parameterValueSegment, CultureInfo.InvariantCulture),
                "double" => double.Parse(parameterValueSegment, CultureInfo.InvariantCulture),
                "float" => float.Parse(parameterValueSegment, CultureInfo.InvariantCulture),
                "guid" => Guid.Parse(parameterValueSegment),
                "int" => int.Parse(parameterValueSegment, CultureInfo.InvariantCulture),
                "long" => long.Parse(parameterValueSegment, CultureInfo.InvariantCulture),
                "timeonly" => TimeOnly.Parse(parameterValueSegment, CultureInfo.InvariantCulture),
                "dateonly" => DateOnly.Parse(parameterValueSegment, CultureInfo.InvariantCulture),
                "string" => parameterValueSegment,
                _ => throw new InvalidDirectiveQueryParameterTypeException(),
            };
        }
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
        queryParameterValue = Uri.UnescapeDataString(queryParameterValue);

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
