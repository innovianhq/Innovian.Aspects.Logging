namespace Innovian.Aspects.Logging.Testing.Aspects
{
  internal class ContainsComplexArgument
  {
    public int DoSomething(Random rand)
    {
      global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ContainsComplexArgument.DoSomething(rand = <non-primitive type>) started");
      var stopwatch = new global::System.Diagnostics.Stopwatch();
      stopwatch.Start();
      try
      {
        global::System.Int32 result;
        result = rand.Next();
        using var guard = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard.CanLog)
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ContainsComplexArgument.DoSomething(rand = <non-primitive type>) succeeded and returned '{result}'");
        return (global::System.Int32)result;
      }
      catch (global::System.Exception ex)
      {
        using var guard_1 = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard_1.CanLog)
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogError(_logger, ex, $"ContainsComplexArgument.DoSomething(rand = <non-primitive type>) failed: {ex.Message}");
        throw;
      }
      finally
      {
        stopwatch.Stop();
        using var guard_2 = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard_2.CanLog)
        {
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ContainsComplexArgument.DoSomething took {{{stopwatch.ElapsedMilliseconds}}} ms to complete");
        }
      }
    }
    private readonly global::Microsoft.Extensions.Logging.ILogger _logger;
    public ContainsComplexArgument(global::Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = default(global::Microsoft.Extensions.Logging.ILoggerFactory? ))
    {
      _logger = loggerFactory is not null ? global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(loggerFactory, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.ContainsComplexArgument)) : global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(global::Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.ContainsComplexArgument));
    }
  }
}