namespace Innovian.Aspects.Logging.Testing.Aspects;
/// <summary>
/// Should inject ILoggerFactory into both constructors, create ILogger field and decorate method with logging.
/// </summary>
public class MultipleConstructors
{
    private string? _name;
    private int? _value;
    public MultipleConstructors(string name, global::Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = default(global::Microsoft.Extensions.Logging.ILoggerFactory?))
    {
        _logger = loggerFactory is not null ? global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(loggerFactory, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.MultipleConstructors)) : global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(global::Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.MultipleConstructors));
        _name = name;
    }
    public MultipleConstructors(int value, global::Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = default(global::Microsoft.Extensions.Logging.ILoggerFactory?))
    {
        _logger = loggerFactory is not null ? global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(loggerFactory, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.MultipleConstructors)) : global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(global::Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.MultipleConstructors));
        _value = value;
    }
    public void DoSomething(string name, int value)
    {
        global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"MultipleConstructors.DoSomething(name = {{{name}}}, value = {{{value}}}) started");
        var stopwatch = new global::System.Diagnostics.Stopwatch();
        stopwatch.Start();
        try
        {
            Console.WriteLine("This method does something!");
            object result = null;
            using var guard = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
            if (guard.CanLog)
                global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"MultipleConstructors.DoSomething(name = {{{name}}}, value = {{{value}}}) succeeded");
            return;
        }
        catch (global::System.Exception ex)
        {
            using var guard_1 = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
            if (guard_1.CanLog)
                global::Microsoft.Extensions.Logging.LoggerExtensions.LogError(_logger, ex, $"MultipleConstructors.DoSomething(name = {{{name}}}, value = {{{value}}}) failed: {ex.Message}");
            throw;
        }
        finally
        {
            stopwatch.Stop();
            using var guard_2 = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
            if (guard_2.CanLog)
            {
                global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"MultipleConstructors.DoSomething took {{{stopwatch.ElapsedMilliseconds}}} ms to complete");
            }
        }
    }
    private readonly global::Microsoft.Extensions.Logging.ILogger _logger;
}