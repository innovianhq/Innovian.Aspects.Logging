using Metalama.Framework.Aspects;
using Metalama.Framework.Code.SyntaxBuilders;
using Metalama.Framework.Code;
using Metalama.Framework.Eligibility;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Innovian.Aspects.Logging;

/// <summary>
/// Introduces logging via the Microsoft.Extensions.Logging client to any methods decorated
/// with this attribute.
/// </summary>
public sealed class LogAttribute : OverrideMethodAspect
{
    /// <summary>
    /// Used for logging and telemetry.
    /// </summary>
    [Introduce(WhenExists = OverrideStrategy.Ignore)]
    private readonly ILogger _logger = default!;

    public override void BuildEligibility(IEligibilityBuilder<IMethod> builder)
    {
        builder.MustSatisfyAll(
            //Must be explicitly declared
            _ => builder.MustBeExplicitlyDeclared(),
            //Must not be static
            _ => builder.MustSatisfy(t => !t.IsStatic, n => $"{n} cannot be static"),
            //Must not be a record type
            _ => builder.DeclaringType().MustSatisfy(t => t.TypeKind is not (TypeKind.RecordStruct or TypeKind.RecordClass),
                x => $"{x} must not be a record"));
    }


    public override Task<dynamic?> OverrideAsyncMethod()
    {
        var entryMessage = BuildMessage();
        entryMessage.AddText(" started");

        LoggerExtensions.LogInformation(_logger, entryMessage.ToExpression().Value);
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        try
        {
            var result = meta.ProceedAsync();

            // https://github.com/postsharp/Metalama/issues/334
            //Display the success message - this message varies when the return is void
            var successMessage = BuildMessage();

            if (meta.Target.Method.ReturnType.Is(typeof(void)))
            {
                //When the method is void, display a constant text
                successMessage.AddText(" succeeded");
            }
            else
            {
                //When the method has a return value, add to the message
                successMessage.AddText(" succeeded and returned '");
                successMessage.AddExpression(result);
                successMessage.AddText("'");
            }

            using var guard = LoggingRecursionGuard.Begin();
            if (guard.CanLog) LoggerExtensions.LogInformation(_logger, successMessage.ToExpression().Value);

            return result;
        }
        catch (Exception ex)
        {
            var message = BuildMessage();
            message.AddText(" failed: ");
            message.AddExpression(ex.Message);

            using var guard = LoggingRecursionGuard.Begin();
            if (guard.CanLog) LoggerExtensions.LogError(_logger, ex, message.ToExpression().Value);

            throw;
        }
        finally
        {
            stopwatch.Stop();
            using var guard = LoggingRecursionGuard.Begin();
            if (guard.CanLog)
            {
                var message = BuildTelemetryMessage();
                message.AddExpression(stopwatch.ElapsedMilliseconds);
                message.AddText("} ms to complete");

                LoggerExtensions.LogInformation(_logger, message.ToExpression().Value);
            }
        }
    }

    /// <summary>
    /// Default template of the new method implementation.
    /// </summary>
    /// <returns></returns>
    public override dynamic? OverrideMethod()
    {
        var entryMessage = BuildMessage();
        entryMessage.AddText(" started");

        LoggerExtensions.LogInformation(_logger, entryMessage.ToExpression().Value);
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        try
        {
            var result = meta.Proceed();

            // https://github.com/postsharp/Metalama/issues/334
            //Display the success message - this message varies when the return is void
            var successMessage = BuildMessage();

            if (meta.Target.Method.ReturnType.Is(typeof(void)))
            {
                //When the method is void, display a constant text
                successMessage.AddText(" succeeded");
            }
            else
            {
                //When the method has a return value, add to the message
                successMessage.AddText(" succeeded and returned ");
                
                if (!IsPrimitive(meta.Target.Method.ReturnType))
                {
                    successMessage.AddText("a non-primitive value");
                }
                else
                {
                    successMessage.AddText("'");
                    successMessage.AddExpression(result);
                }

                successMessage.AddText("'");
            }

            using var guard = LoggingRecursionGuard.Begin();
            if (guard.CanLog) LoggerExtensions.LogInformation(_logger, successMessage.ToExpression().Value);

            return result;
        }
        catch (Exception ex)
        {
            var message = BuildMessage();
            message.AddText(" failed: ");
            message.AddExpression(ex.Message);

            using var guard = LoggingRecursionGuard.Begin();
            if (guard.CanLog) LoggerExtensions.LogError(_logger, ex, message.ToExpression().Value);

            throw;
        }
        finally
        {
            stopwatch.Stop();
            using var guard = LoggingRecursionGuard.Begin();
            if (guard.CanLog)
            {
                var message = BuildTelemetryMessage();
                message.AddExpression(stopwatch.ElapsedMilliseconds);
                message.AddText("} ms to complete");

                LoggerExtensions.LogInformation(_logger, message.ToExpression().Value);
            }
        }
    }

    /// <summary>
    /// Determines whether a given type is a primitive or not.
    /// </summary>
    /// <param name="type">The type to evaluate.</param>
    /// <returns>True if the type is primitive, false if not.</returns>
    private static bool IsPrimitive(IType type) =>
        type.SpecialType is SpecialType.Boolean or SpecialType.Byte or SpecialType.SByte or SpecialType.Int16
            or SpecialType.Int32 or SpecialType.Int64 or SpecialType.UInt16 or SpecialType.UInt32 or SpecialType.UInt64
            or SpecialType.Char or SpecialType.Double or SpecialType.Decimal or SpecialType.Single
            or SpecialType.String;

    /// <summary>
    /// Builds the string detailing how long it took the method to run.
    /// </summary>
    /// <returns></returns>
    private static InterpolatedStringBuilder BuildTelemetryMessage()
    {
        var stringBuilder = new InterpolatedStringBuilder();

        stringBuilder.AddText(meta.Target.Type.ToDisplayString(CodeDisplayFormat.MinimallyQualified));
        stringBuilder.AddText(".");
        stringBuilder.AddText(meta.Target.Method.Name);
        stringBuilder.AddText(" took {");

        return stringBuilder;
    }

    /// <summary>
    /// Builds the string comprising parameters passed into the method.
    /// </summary>
    /// <param name="includeOutParameters">True if the out parameters should be included, false if not.</param>
    /// <returns></returns>
    private static InterpolatedStringBuilder BuildMessage(bool includeOutParameters = false)
    {
        var stringBuilder = new InterpolatedStringBuilder();

        stringBuilder.AddText(meta.Target.Type.ToDisplayString(CodeDisplayFormat.MinimallyQualified));
        stringBuilder.AddText(".");
        stringBuilder.AddText(meta.Target.Method.Name);
        stringBuilder.AddText("(");
        var i = 0;

        //Include a placeholder for each parameter
        foreach (var p in meta.Target.Parameters)
        {
            var comma = i > 0 ? ", " : "";

            if (p.RefKind == RefKind.Out && !includeOutParameters)
            {
                //When the parameter is 'out', we cannot read the value
                stringBuilder.AddText($"{comma}{p.Name} = <out>");
            }
            else if (IsSensitive(p))
            {
                //When the parameter is decorated as a secret, we _should_ not read that value
                //Instead, replace with the value from the attribute
                var attr = p.Attributes.OfAttributeType(typeof(SensitiveAttribute)).FirstOrDefault() as SensitiveAttribute;
                var replacementValue = attr != default
                    ? attr.ReplacementText
                    : SensitiveAttribute.DefaultReplacementValue;

                stringBuilder.AddText($"{comma}{p.Name} = '{replacementValue}'");
            }
            else if (!IsPrimitive(p.Type))
            {
                stringBuilder.AddText($"{comma}{p.Name} = <non-primitive type>");
            }
            else
            {
                //Otherwise, add the parameter value
                stringBuilder.AddText($"{comma}{p.Name} = {{");
                stringBuilder.AddExpression(p);
                stringBuilder.AddText("}");
            }

            i++;
        }

        stringBuilder.AddText(")");

        return stringBuilder;
    }

    /// <summary>
    /// Determines if the <see cref="SensitiveAttribute" /> is applied to the parameter or not.
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    private static bool IsSensitive(IParameter parameter)
    {
        return parameter.Attributes.OfAttributeType(typeof(SensitiveAttribute)).Any();
    }
}