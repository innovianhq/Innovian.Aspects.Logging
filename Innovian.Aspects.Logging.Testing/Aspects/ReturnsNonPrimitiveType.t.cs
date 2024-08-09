namespace Innovian.Aspects.Logging.Testing.Aspects
{
  internal class ReturnsNonPrimitiveType
  {
    public Doodad DoSomething()
    {
      global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ReturnsNonPrimitiveType.DoSomething() started");
      var stopwatch = new global::System.Diagnostics.Stopwatch();
      stopwatch.Start();
      try
      {
        global::Innovian.Aspects.Logging.Testing.Aspects.Doodad result;
        result = new Doodad();
        using var guard = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard.CanLog)
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ReturnsNonPrimitiveType.DoSomething() succeeded and returned a non-primitive value'");
        return (global::Innovian.Aspects.Logging.Testing.Aspects.Doodad)result;
      }
      catch (global::System.Exception ex)
      {
        using var guard_1 = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard_1.CanLog)
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogError(_logger, ex, $"ReturnsNonPrimitiveType.DoSomething() failed: {ex.Message}");
        throw;
      }
      finally
      {
        stopwatch.Stop();
        using var guard_2 = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard_2.CanLog)
        {
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ReturnsNonPrimitiveType.DoSomething took {{{stopwatch.ElapsedMilliseconds}}} ms to complete");
        }
      }
    }
    private readonly global::Microsoft.Extensions.Logging.ILogger _logger;
    public ReturnsNonPrimitiveType(global::Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = default(global::Microsoft.Extensions.Logging.ILoggerFactory? ))
    {
      _logger = loggerFactory is not null ? global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(loggerFactory, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.ReturnsNonPrimitiveType)) : global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(global::Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.ReturnsNonPrimitiveType));
    }
  }
  internal record Doodad();
}