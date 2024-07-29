namespace Innovian.Aspects.Logging.Testing.Aspects;

/// <summary>
/// Should inject ILoggerFactory into both constructors, create ILogger field and decorate method with logging.
/// </summary>
public class MultipleConstructors
{
    private string? _name;
    private int? _value;

    public MultipleConstructors(string name)
    {
        _name = name;
    }

    public MultipleConstructors(int value)
    {
        _value = value;
    }

    public void DoSomething(string name, int value)
    {
        Console.WriteLine("This method does something!");
    }
}