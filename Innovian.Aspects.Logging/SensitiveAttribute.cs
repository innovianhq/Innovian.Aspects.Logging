using Metalama.Framework.Aspects;

namespace Innovian.Aspects.Logging;

/// <summary>
/// Marks a parameter as sensitive, meaning that its value should not be included in logs.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.Method |
                AttributeTargets.Class)]
[RunTimeOrCompileTime]
public class SensitiveAttribute : Attribute
{
    /// <summary>
    /// Shares the default replacement value publicly.
    /// </summary>
    public static string DefaultReplacementValue = "***";

    /// <summary>
    /// Indicates that a static replacement of the sensitive text should occur.
    /// </summary>
    public SensitiveAttribute()
    {
        ReplacementText = DefaultReplacementValue;
    }

    /// <summary>
    /// Indicates that a static replacement of the sensitive text should occur.
    /// </summary>
    /// <param name="replacementText"></param>
    public SensitiveAttribute(string replacementText)
    {
        ReplacementText = replacementText;
    }

    /// <summary>
    /// The value of the text to replace the sensitive value with.
    /// </summary>
    public string ReplacementText { get; }
}