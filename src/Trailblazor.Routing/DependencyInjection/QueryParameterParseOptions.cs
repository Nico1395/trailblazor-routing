using System.Globalization;

namespace Trailblazor.Routing.DependencyInjection;

/// <summary>
/// Options for parsing query parameter values.
/// </summary>
public class QueryParameterParseOptions
{
    /// <summary>
    /// Query parameter values are being parsed as a string by default. This setting determines whether to attempt to parse the values into
    /// primitive types or not.
    /// </summary>
    /// <remarks>
    /// This setting is set to <see langword="false"/> by default, string values are being parsed into primitive types.
    /// </remarks>
    public bool DontParseToPrimitiveTypes { get; set; }

    /// <summary>
    /// Function for configuring the <see cref="CultureInfo"/> that is to be used as a <see cref="IFormatProvider"/> when parsing numeric values.
    /// </summary>
    /// <remarks>
    /// This setting is set to <see cref="CultureInfo.CurrentCulture"/> by default.
    /// </remarks>
    public Func<CultureInfo> NumericParseCultureInfo { get; set; } = () => CultureInfo.CurrentCulture;

    /// <summary>
    /// Function for configuring the <see cref="CultureInfo"/> that is to be used as a <see cref="IFormatProvider"/> when parsing time units such
    /// as <see cref="DateTime"/>, <see cref="TimeOnly"/> and <see cref="DateOnly"/>.
    /// </summary>
    /// <remarks>
    /// This setting is set to <see cref="CultureInfo.CurrentCulture"/> by default.
    /// </remarks>
    public Func<CultureInfo> DateTimeParseCultureInfo { get; set; } = () => CultureInfo.CurrentCulture;

    /// <summary>
    /// Styles for parsing time units such as <see cref="DateTime"/>, <see cref="TimeOnly"/> and <see cref="DateOnly"/>.
    /// </summary>
    /// <remarks>
    /// This setting is set to <see cref="DateTimeStyles.None"/> by default.
    /// </remarks>
    public DateTimeStyles DateTimeStyles { get; set; }

    /// <summary>
    /// Factory method creates default parse options.
    /// </summary>
    /// <returns>Default parse options.</returns>
    public static QueryParameterParseOptions Default()
    {
        return new QueryParameterParseOptions()
        {
            DontParseToPrimitiveTypes = false,
            DateTimeStyles = DateTimeStyles.None,
        };
    }
}
