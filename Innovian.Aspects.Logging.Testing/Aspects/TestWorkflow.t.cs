using Dapr.Workflow;
namespace Innovian.Aspects.Logging.Testing.Aspects;
internal sealed class TestWorkflow : Workflow<Guid, object?>
{
    /// <summary>Override to implement workflow logic.</summary>
    /// <param name = "context">The workflow context.</param>
    /// <param name = "input">The deserialized workflow input.</param>
    /// <returns>The output of the workflow as a task.</returns>
    public override async Task<object?> RunAsync(WorkflowContext context, Guid input)
    {
        throw new NotImplementedException();
    }
}