namespace Innovian.Aspects.Logging.Testing.Aspects;

/// <summary>
/// Should inject ILoggerFactory and create ILogger field, but should not wrap method with logging since it's static.
/// </summary>
public class StaticMethod
{
    public static void ThisMethod()
    {
        Console.WriteLine("This method does something!");
    }
}