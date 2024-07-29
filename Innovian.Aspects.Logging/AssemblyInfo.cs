using Innovian.Aspects.Logging;
using Metalama.Framework.Aspects;

[assembly: AspectOrder(AspectOrderDirection.RunTime, typeof(LogAttribute), typeof(InjectLoggerFactoryAttribute), typeof(InjectLoggerAttribute))]
