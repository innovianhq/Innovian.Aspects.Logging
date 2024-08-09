namespace Innovian.Aspects.Logging.Testing.Aspects
{
  internal class ReturnsPrimitiveType
  {
    public int ReturnsPrimitiveTypeValue()
    {
      global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ReturnsPrimitiveType.ReturnsPrimitiveTypeValue() started");
      var stopwatch = new global::System.Diagnostics.Stopwatch();
      stopwatch.Start();
      try
      {
        global::System.Int32 result;
        result = 5;
        using var guard = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard.CanLog)
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ReturnsPrimitiveType.ReturnsPrimitiveTypeValue() succeeded and returned '{result}'");
        return (global::System.Int32)result;
      }
      catch (global::System.Exception ex)
      {
        using var guard_1 = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard_1.CanLog)
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogError(_logger, ex, $"ReturnsPrimitiveType.ReturnsPrimitiveTypeValue() failed: {ex.Message}");
        throw;
      }
      finally
      {
        stopwatch.Stop();
        using var guard_2 = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard_2.CanLog)
        {
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ReturnsPrimitiveType.ReturnsPrimitiveTypeValue took {{{stopwatch.ElapsedMilliseconds}}} ms to complete");
        }
      }
    }
    private readonly global::Microsoft.Extensions.Logging.ILogger _logger;
    public ReturnsPrimitiveType(global::Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = default(global::Microsoft.Extensions.Logging.ILoggerFactory? ))
    {
      _logger = loggerFactory is not null ? global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(loggerFactory, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.ReturnsPrimitiveType)) : global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(global::Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.ReturnsPrimitiveType));
    }
  }
}