namespace Innovian.Aspects.Logging.Testing.Aspects;

/// <summary>
/// Should not inject ILoggerFactory or create ILogger or wrap method with logging since it's a static class
/// </summary>
public static class StaticClass
{
    public static void ThisMethod()
    {
        Console.WriteLine("This method does something!");
    }
}