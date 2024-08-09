namespace Innovian.Aspects.Logging.Testing.Aspects
{
  internal class ReturnListTypeInMethod
  {
    public List<int> DoSomething()
    {
      global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ReturnListTypeInMethod.DoSomething() started");
      var stopwatch = new global::System.Diagnostics.Stopwatch();
      stopwatch.Start();
      try
      {
        global::System.Collections.Generic.List<global::System.Int32> result;
        var numbers = new List<int>();
        for (var a = 0; a < 1000; a++)
        {
          numbers.Add(a);
        }
        result = numbers;
        using var guard = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard.CanLog)
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ReturnListTypeInMethod.DoSomething() succeeded and returned a non-primitive value'");
        return (global::System.Collections.Generic.List<global::System.Int32>)result;
      }
      catch (global::System.Exception ex)
      {
        using var guard_1 = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard_1.CanLog)
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogError(_logger, ex, $"ReturnListTypeInMethod.DoSomething() failed: {ex.Message}");
        throw;
      }
      finally
      {
        stopwatch.Stop();
        using var guard_2 = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard_2.CanLog)
        {
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ReturnListTypeInMethod.DoSomething took {{{stopwatch.ElapsedMilliseconds}}} ms to complete");
        }
      }
    }
    private readonly global::Microsoft.Extensions.Logging.ILogger _logger;
    public ReturnListTypeInMethod(global::Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = default(global::Microsoft.Extensions.Logging.ILoggerFactory? ))
    {
      _logger = loggerFactory is not null ? global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(loggerFactory, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.ReturnListTypeInMethod)) : global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(global::Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.ReturnListTypeInMethod));
    }
  }
}