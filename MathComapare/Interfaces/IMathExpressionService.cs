

namespace MathComapare.Models
{
    public interface IMathExpressionService
    {
        Task<GenerateExpressionResponse> GenerateExpressions(string difficulty);
        Task<MathResult> EvaluateComparison(ComparisionRequest request);
    }
}
