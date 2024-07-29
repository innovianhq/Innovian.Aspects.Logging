namespace Innovian.Aspects.Logging;

/// <summary>
/// Used in the prevention of stack overflows resulting from infinitely looped called to ToString and other
/// such methods with default framework-provided implementations and overrides.
/// </summary>
public static class LoggingRecursionGuard
{
    /// <summary>
    /// Indicates whether the guard is or isn't already logging something, preempting
    /// a nested method from also being logged.
    /// </summary>
    [ThreadStatic]
    public static bool IsLogging;

    /// <summary>
    /// Indicates an entry attempt to log (or not) that checks to see if it's eligible to do so or not.
    /// </summary>
    /// <returns></returns>
    public static DisposeCookie Begin()
    {
        if (IsLogging)
        {
            return new DisposeCookie(false);
        }
        else
        {
            IsLogging = true;
            return new DisposeCookie(true);
        }
    }

    /// <summary>
    /// Maintains a disposable cookie that keeps track of whether the logging can proceed or not.
    /// </summary>
    public class DisposeCookie : IDisposable
    {
        /// <summary>
        /// Indicates whether it's safe to log or not.
        /// </summary>
        public bool CanLog { get; }

        /// <summary>
        /// Instantiates the <see cref="DisposeCookie"/> with a value indicating whether it's safe to log or not.
        /// </summary>
        /// <param name="canLog"></param>
        public DisposeCookie(bool canLog)
        {
            CanLog = canLog;
        }

        /// <summary>
        /// Implementation of <see cref="IDisposable"/>.
        /// </summary>
#pragma warning disable CA1816
        public void Dispose()
#pragma warning restore CA1816
        {
            if (CanLog)
            {
                IsLogging = false;
            }
        }
    }
}