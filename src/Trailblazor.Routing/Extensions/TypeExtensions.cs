namespace Trailblazor.Routing.Extensions;

internal static class TypeExtensions
{
    internal static bool IsString(this Type type)
    {
        return type == typeof(string);
    }

    internal static bool IsGuid(this Type type)
    {
        return type == typeof(Guid) || type == typeof(Guid?);
    }

    internal static bool IsDateTime(this Type type)
    {
        return type == typeof(DateTime) || type == typeof(DateTime?);
    }

    internal static bool IsTimeOnly(this Type type)
    {
        return type == typeof(TimeOnly) || type == typeof(TimeOnly?);
    }

    internal static bool IsDateOnly(this Type type)
    {
        return type == typeof(DateOnly) || type == typeof(DateOnly?);
    }

    internal static bool IsBool(this Type type)
    {
        return type == typeof(bool);
    }

    internal static bool IsInt(this Type type)
    {
        return type == typeof(int) || type == typeof(int?);
    }

    internal static bool IsDouble(this Type type)
    {
        return type == typeof(double) || type == typeof(double?);
    }

    internal static bool IsLong(this Type type)
    {
        return type == typeof(long) || type == typeof(long?);
    }

    internal static bool IsDecimal(this Type type)
    {
        return type == typeof(decimal) || type == typeof(decimal?);
    }
}
