namespace Dumbogram.Common.Utilities;

public static class EnumUtility
{
    public static IEnumerable<T> GetValues<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }
}