using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Innovian.Aspects.Logging.Testing.Aspects;

/// <summary>
/// Should not re-inject ILogger field since it already exists or ILoggerFactory since they already exist,
/// but should wrap method with logging.
/// </summary>
/// <param name="loggerFactory"></param>
public class DoubleInjection(ILoggerFactory? loggerFactory)
{
    private ILogger _logger = loggerFactory?.CreateLogger<DoubleInjection>() ??
                              NullLoggerFactory.Instance.CreateLogger<DoubleInjection>();

    public void ShouldNotReinject()
    {
        Console.WriteLine("Whoops");
    }
}