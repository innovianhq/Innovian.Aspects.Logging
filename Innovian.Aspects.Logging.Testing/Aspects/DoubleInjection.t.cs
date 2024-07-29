using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
namespace Innovian.Aspects.Logging.Testing.Aspects;
/// <summary>
/// Should not re-inject ILogger field since it already exists or ILoggerFactory since they already exist,
/// but should wrap method with logging.
/// </summary>
/// <param name = "loggerFactory"></param>
public class DoubleInjection(ILoggerFactory? loggerFactory)
{
    private ILogger _logger = loggerFactory?.CreateLogger<DoubleInjection>() ?? NullLoggerFactory.Instance.CreateLogger<DoubleInjection>();
    public void ShouldNotReinject()
    {
        global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"DoubleInjection.ShouldNotReinject() started");
        var stopwatch = new global::System.Diagnostics.Stopwatch();
        stopwatch.Start();
        try
        {
            Console.WriteLine("Whoops");
            object result = null;
            using var guard = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
            if (guard.CanLog)
                global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"DoubleInjection.ShouldNotReinject() succeeded");
            return;
        }
        catch (global::System.Exception ex)
        {
            using var guard_1 = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
            if (guard_1.CanLog)
                global::Microsoft.Extensions.Logging.LoggerExtensions.LogError(_logger, ex, $"DoubleInjection.ShouldNotReinject() failed: {ex.Message}");
            throw;
        }
        finally
        {
            stopwatch.Stop();
            using var guard_2 = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
            if (guard_2.CanLog)
            {
                global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"DoubleInjection.ShouldNotReinject took {{{stopwatch.ElapsedMilliseconds}}} ms to complete");
            }
        }
    }
}