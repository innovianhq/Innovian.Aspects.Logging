namespace Innovian.Aspects.Logging.Testing.Aspects;

/// <summary>
/// Should inject ILoggerFactory into constructor (re-realized as explicit instead of primary constructor),
/// create ILogger field and decorate method with logging.
/// </summary>
public class StandardExample(int value)
{
    public int Value { get; init; } = value;

    public void DoSomething(string name, int value)
    {
        Console.WriteLine("This method does something!");
    }
}