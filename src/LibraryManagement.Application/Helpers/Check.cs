namespace LibraryManagement.Application.Helpers;

public static class Check
{
    public static void Empty(string paramValue, string paramName)
    {
        if (string.IsNullOrEmpty(paramValue))
            throw new ArgumentNullException(paramName);
    }

    public static void Empty(Guid? paramValue, string paramName)
    {
        if (paramValue is null || paramValue == Guid.Empty)
            throw new ArgumentNullException(paramName);
    }
}
