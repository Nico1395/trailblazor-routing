using System.Globalization;
using Trailblazor.Routing.DependencyInjection;

namespace Trailblazor.Routing;

internal sealed class QueryParameterParser(RoutingOptions _routingOptions) : IQueryParameterParser
{
    private CultureInfo? _numericParseCultureInfo;
    private CultureInfo? _dateTimeParseCultureInfo;

    private CultureInfo NumericParseCultureInfo => _numericParseCultureInfo ??= _routingOptions.QueryParameterParseOptions.NumericParseCultureInfo();
    private CultureInfo DateTimeParseCultureInfo => _dateTimeParseCultureInfo ??= _routingOptions.QueryParameterParseOptions.DateTimeParseCultureInfo();

    public object ParseValue(string stringValue)
    {
        if (_routingOptions.QueryParameterParseOptions.DontParseToPrimitiveTypes)
            return stringValue;

        return AttemptToParseToPrimitiveTypes(stringValue);
    }

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

        if (DateTime.TryParse(stringValue, DateTimeParseCultureInfo, _routingOptions.QueryParameterParseOptions.DateTimeStyles, out var dateTimeValue))
            return dateTimeValue;

        if (TimeOnly.TryParse(stringValue, DateTimeParseCultureInfo, _routingOptions.QueryParameterParseOptions.DateTimeStyles, out var timeOnlyValue))
            return timeOnlyValue;

        if (DateOnly.TryParse(stringValue, DateTimeParseCultureInfo, _routingOptions.QueryParameterParseOptions.DateTimeStyles, out var dateOnlyValue))
            return dateOnlyValue;

        return stringValue;
    }
}
