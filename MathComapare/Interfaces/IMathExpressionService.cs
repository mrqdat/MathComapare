

namespace MathComapare.Models
{
    public interface IMathExpressionService
    {
        Task<GenerateExpressionResponse> GenerateExpressions(int difficulty);
        Task<MathResult> EvaluateComparison(ComparisionRequest request);
    }
}
