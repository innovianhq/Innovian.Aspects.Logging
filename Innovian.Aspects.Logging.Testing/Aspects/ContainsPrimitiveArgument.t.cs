using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Innovian.Aspects.Logging.Testing.Aspects
{
  internal class ContainsPrimitiveArgument
  {
    public int DoSomething(int number, string name)
    {
      global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ContainsPrimitiveArgument.DoSomething(number = {{{number}}}, name = {{{name}}}) started");
      var stopwatch = new global::System.Diagnostics.Stopwatch();
      stopwatch.Start();
      try
      {
        global::System.Int32 result;
        Console.WriteLine("Expected a name {name} and a number {number}", name, number);
        result = number;
        using var guard = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard.CanLog)
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ContainsPrimitiveArgument.DoSomething(number = {{{number}}}, name = {{{name}}}) succeeded and returned '{result}'");
        return (global::System.Int32)result;
      }
      catch (global::System.Exception ex)
      {
        using var guard_1 = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard_1.CanLog)
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogError(_logger, ex, $"ContainsPrimitiveArgument.DoSomething(number = {{{number}}}, name = {{{name}}}) failed: {ex.Message}");
        throw;
      }
      finally
      {
        stopwatch.Stop();
        using var guard_2 = global::Innovian.Aspects.Logging.LoggingRecursionGuard.Begin();
        if (guard_2.CanLog)
        {
          global::Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(_logger, $"ContainsPrimitiveArgument.DoSomething took {{{stopwatch.ElapsedMilliseconds}}} ms to complete");
        }
      }
    }
    private readonly global::Microsoft.Extensions.Logging.ILogger _logger;
    public ContainsPrimitiveArgument(global::Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = default(global::Microsoft.Extensions.Logging.ILoggerFactory? ))
    {
      _logger = loggerFactory is not null ? global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(loggerFactory, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.ContainsPrimitiveArgument)) : global::Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger(global::Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance, typeof(global::Innovian.Aspects.Logging.Testing.Aspects.ContainsPrimitiveArgument));
    }
  }
}