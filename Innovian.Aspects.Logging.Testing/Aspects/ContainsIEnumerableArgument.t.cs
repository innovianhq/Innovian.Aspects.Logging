using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Innovian.Aspects.Logging.Testing.Aspects
{
  internal class ContainsIEnumerableArgument
  {
    public void DoSomething(List<int> numbers)
    {
      global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ContainsIEnumerableArgument.DoSomething(numbers = <non-primitive type>) started");
      var stopwatch = new global::System.Diagnostics.Stopwatch();
      stopwatch.Start();
      try
      {
        Console.WriteLine($"There are {numbers.Count} numbers!");
        object result = null;
        using var guard = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard.CanLog)
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ContainsIEnumerableArgument.DoSomething(numbers = <non-primitive type>) succeeded");
        return;
      }
      catch (global::System.Exception ex)
      {
        using var guard_1 = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard_1.CanLog)
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogError(_logger, ex, $"ContainsIEnumerableArgument.DoSomething(numbers = <non-primitive type>) failed: {ex.Message}");
        throw;
      }
      finally
      {
        stopwatch.Stop();
        using var guard_2 = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard_2.CanLog)
        {
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ContainsIEnumerableArgument.DoSomething took {{{stopwatch.ElapsedMilliseconds}}} ms to complete");
        }
      }
    }
    private readonly global::Microsoft.Extensions.Logging.ILogger _logger;
    public ContainsIEnumerableArgument(global::Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = default(global::Microsoft.Extensions.Logging.ILoggerFactory? ))
    {
      _logger = loggerFactory is not null ? global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(loggerFactory, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.ContainsIEnumerableArgument)) : global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(global::Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.ContainsIEnumerableArgument));
    }
  }
}