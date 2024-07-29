namespace Innovian.Aspects.Logging.Testing.Aspects;

/// <summary>
/// Should not inject anything and should not wrap method with logging since it's a record.
/// </summary>
public record RecordType
{
    public void ShouldNotLogRecords()
    {
        Console.WriteLine("This method does NOTHING!");
    }
}