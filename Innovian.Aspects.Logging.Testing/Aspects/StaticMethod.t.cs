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
    private readonly global::Microsoft.Extensions.Logging.ILogger _logger;
    public StaticMethod(global::Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = default(global::Microsoft.Extensions.Logging.ILoggerFactory?))
    {
        _logger = loggerFactory is not null ? global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(loggerFactory, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.StaticMethod)) : global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(global::Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.StaticMethod));
    }
}