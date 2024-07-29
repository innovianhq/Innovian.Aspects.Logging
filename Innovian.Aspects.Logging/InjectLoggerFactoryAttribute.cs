using Metalama.Framework.Aspects;
using Metalama.Framework.Code.SyntaxBuilders;
using Metalama.Framework.Code;
using Metalama.Framework.Eligibility;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Metalama.Framework.Advising;

namespace Innovian.Aspects.Logging;

/// <summary>
/// Custom attribute that, when applied to a type's constructor, means that the constructor should be
/// updated to include an <see cref="ILoggerFactory"/>, if it doesn't already exist.
/// </summary>
public sealed class InjectLoggerFactoryAttribute : ConstructorAspect
{
    /// <inheritdoc />
    public override void BuildEligibility(IEligibilityBuilder<IConstructor> builder)
    {
        builder.MustSatisfyAll(
            //Must be explicitly declared
            _ => builder.MustBeExplicitlyDeclared(),
            //Must not be static
            _ => builder.MustSatisfy(t => !t.IsStatic, n => $"The method cannot be static"),
            //Must not be a record type
            _ => builder.DeclaringType().MustSatisfy(t => t.TypeKind is not (TypeKind.RecordStruct or TypeKind.RecordClass),
                x => $"The declaring type must not be a record"));
    }

    public override void BuildAspect(IAspectBuilder<IConstructor> builder)
    {
        var nullableLoggerFactoryTypeFactory = (INamedType)TypeFactory.GetType(typeof(ILoggerFactory)).ToNullableType();

        //Skip the implementation if the ILoggerFactory? or ILoggerFactory already exists in the constructor parameters 
        if (builder.Target.Parameters.Any(parameter =>
                parameter.GetType() == nullableLoggerFactoryTypeFactory.GetType()
                || parameter.GetType() == typeof(ILoggerFactory)
                || parameter.Name == "loggerFactory"))
        {
            return;
        }

        builder.Advice.IntroduceParameter(
            builder.Target,
            "loggerFactory",
            nullableLoggerFactoryTypeFactory,
            TypedConstant.CreateUnchecked(null, nullableLoggerFactoryTypeFactory),
            pullAction: (parameter, constructor) =>
                PullAction.IntroduceParameterAndPull(
                    "loggerFactory",
                    nullableLoggerFactoryTypeFactory,
                    TypedConstant.CreateUnchecked(null, nullableLoggerFactoryTypeFactory))
        );


        var exprBuilder = new ExpressionBuilder();

        exprBuilder.AppendVerbatim("_logger = loggerFactory is not null ? ");
        exprBuilder.AppendTypeName(typeof(LoggerFactoryExtensions));
        exprBuilder.AppendVerbatim(".CreateLogger(loggerFactory, typeof(");
        exprBuilder.AppendTypeName(builder.Target.DeclaringType);
        exprBuilder.AppendVerbatim(")) : ");

        exprBuilder.AppendTypeName(typeof(LoggerFactoryExtensions));
        exprBuilder.AppendVerbatim(".CreateLogger(");
        exprBuilder.AppendTypeName(((INamedType)TypeFactory.GetType(typeof(NullLoggerFactory))).ToType());
        exprBuilder.AppendVerbatim(".Instance, typeof(");
        exprBuilder.AppendTypeName(builder.Target.DeclaringType);
        exprBuilder.AppendVerbatim("))");

        builder.Advice.AddInitializer(builder.Target, StatementFactory.FromExpression(exprBuilder.ToExpression()));
    }
}