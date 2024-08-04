using System.Globalization;
using Trailblazor.Routing.DependencyInjection;

namespace Trailblazor.Routing;

/// <summary>
/// Service parses query parameter values from strings into their respective type.
/// </summary>
internal sealed class QueryParameterParser(IRoutingOptionsProvider _routingOptionsProvider) : IQueryParameterParser
{
    private CultureInfo? _numericParseCultureInfo;
    private CultureInfo? _dateTimeParseCultureInfo;

    private CultureInfo NumericParseCultureInfo => _numericParseCultureInfo ??= _routingOptionsProvider.GetRoutingOptions().QueryParameterParseOptions.NumericParseCultureInfo();
    private CultureInfo DateTimeParseCultureInfo => _dateTimeParseCultureInfo ??= _routingOptionsProvider.GetRoutingOptions().QueryParameterParseOptions.DateTimeParseCultureInfo();

    /// <summary>
    /// Method attempts to parse the specified <paramref name="stringValue"/>. If the string could not be parsed into
    /// any primitive type or standard struct such as <see cref="Guid"/> or <see cref="DateTime"/>, then the specified
    /// <paramref name="stringValue"/> will be returned.
    /// </summary>
    /// <param name="stringValue">Query parameter value as a string to be parsed.</param>
    /// <returns>Parsed <paramref name="stringValue"/>.</returns>
    public object ParseValue(string stringValue)
    {
        if (_routingOptionsProvider.GetRoutingOptions().QueryParameterParseOptions.DontParseToPrimitiveTypes)
            return stringValue;

        return AttemptToParseToPrimitiveTypes(stringValue);
    }

    /// <summary>
    /// Method attempts to parse the <paramref name="stringValue"/> into primitive types or standard structs.
    /// </summary>
    /// <param name="stringValue">Value string to be parsed.</param>
    /// <returns>Parsed <paramref name="stringValue"/>.</returns>
    private object AttemptToParseToPrimitiveTypes(string stringValue)
    {
        if (bool.TryParse(stringValue, out var boolValue))
            return boolValue;

        if (int.TryParse(stringValue, NumericParseCultureInfo, out var intValue))
            return intValue;

        if (long.TryParse(stringValue, NumericParseCultureInfo, out var longValue))
            return longValue;

        if (double.TryParse(stringValue, NumericParseCultureInfo, out var doubleValue))
            return doubleValue;

        if (decimal.TryParse(stringValue, NumericParseCultureInfo, out var decimalValue))
            return decimalValue;

        if (Guid.TryParse(stringValue, out var guidValue))
            return guidValue;

        if (DateTime.TryParse(stringValue, DateTimeParseCultureInfo, _routingOptionsProvider.GetRoutingOptions().QueryParameterParseOptions.DateTimeStyles, out var dateTimeValue))
            return dateTimeValue;

        if (TimeOnly.TryParse(stringValue, DateTimeParseCultureInfo, _routingOptionsProvider.GetRoutingOptions().QueryParameterParseOptions.DateTimeStyles, out var timeOnlyValue))
            return timeOnlyValue;

        if (DateOnly.TryParse(stringValue, DateTimeParseCultureInfo, _routingOptionsProvider.GetRoutingOptions().QueryParameterParseOptions.DateTimeStyles, out var dateOnlyValue))
            return dateOnlyValue;

        return stringValue;
    }
}
