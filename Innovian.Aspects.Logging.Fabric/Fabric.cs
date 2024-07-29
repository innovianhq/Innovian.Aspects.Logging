using Metalama.Framework.Fabrics;

namespace Innovian.Aspects.Logging.Fabric;

/// <summary>
/// Applies a fabric to the project referencing this that injects the appropriate types for Microsoft.Extensions.Logging and
/// then wraps all targeted methods with logging.
/// </summary>
public sealed class Fabric : TransitiveProjectFabric
{
    /// <summary>
    /// The user can implement this method to analyze types in the current project, add aspects, and report or suppress diagnostics.
    /// </summary>
    public override void AmendProject(IProjectAmender amender)
    {
        amender.SelectMany(p => p.Types)
            .Where(t => t.Methods.Count > 0) //Should have at least one method to decorate or no point
            //.Where(t => t.BaseType?.Name != "Workflow") //Should not log within Dapr Workflow instances
            .AddAspectIfEligible<InjectLoggerAttribute>();
    }
}