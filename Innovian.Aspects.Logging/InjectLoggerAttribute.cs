using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Eligibility;
using Microsoft.Extensions.Logging;

namespace Innovian.Aspects.Logging;

/// <summary>
/// Custom attribute that, when applied to a type, means that a field should be introduced to the type
/// building out the appropriate <see cref="ILogger"/> instance.
/// </summary>
public sealed class InjectLoggerAttribute : TypeAspect
{
    /// <inheritdoc />
    public override void BuildEligibility(IEligibilityBuilder<INamedType> builder)
    {
        builder.MustSatisfyAll(
            //Must be explicitly declared
            _ => builder.MustBeExplicitlyDeclared(),
            //Must not be static
            _ => builder.MustSatisfy(t => !t.IsStatic, n => $"{n} cannot be static"),
            //Must not be a record type
            _ => builder.DeclaringType().MustSatisfy(
                t => t.TypeKind is not (TypeKind.RecordStruct or TypeKind.RecordClass),
                x => $"{x} must not be a record"));
    }

    /// <inheritdoc />
    public override void BuildAspect(IAspectBuilder<INamedType> builder)
    {
        //Only apply the ILogger field if it doesn't already exist on the type
        //Can't put this in eligibility since this aspect is responsible for chaining the other two aspects as well below
        if (!builder.Target.AllFieldsAndProperties.Any(fp => fp.Name == "_logger" && fp.Type.Is(typeof(ILogger))))
        {

            //Introduce the field to the type
            var loggerTypeFactory = (INamedType)TypeFactory.GetType(typeof(ILogger));
            builder.Advice.IntroduceField(
                builder.Target,
                "_logger",
                loggerTypeFactory,
                IntroductionScope.Instance,
                whenExists: OverrideStrategy.Ignore,
                buildField: b =>
                {
                    b.Accessibility = Accessibility.Private;
                    b.Writeability = Writeability.ConstructorOnly;
                });

            //If only a default (implicit) constructor, explicitly add one so we can decorate it with the attribute later
            if (builder.Target.HasDefaultConstructor)
            {
                builder.Advice.IntroduceConstructor(builder.Target, nameof(TypeConstructorTemplate));
            }
        }

        builder.Outbound.SelectMany(a => a.Constructors)
            .AddAspectIfEligible<InjectLoggerFactoryAttribute>();
        builder.Outbound.SelectMany(a => a.Methods)
            .AddAspectIfEligible<LogAttribute>();
    }

    [Template]
    public void TypeConstructorTemplate()
    {
    }
}