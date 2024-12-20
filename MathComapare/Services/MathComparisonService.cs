using Mathos.Parser;

namespace MathComapare.Models
{
    public class MathComparisonService : IMathExpressionService
    {
        private static readonly Random random = new();

        public async Task<MathResult> EvaluateComparison(ComparisionRequest request)
        {
            var value1 = string.IsNullOrEmpty(request.Expression1) == true ? 0 : await EvaluateExpression(request.Expression1);
            var value2 = string.IsNullOrEmpty(request.Expression2) == true ? 0 : await EvaluateExpression(request.Expression2);
            var res = new MathResult()
            {
                IsValid = request.Operator switch
                {
                    "<" => value1 < value2,
                    ">" => value1 > value2,
                    "=" => Math.Abs(value1 - value2) < 0.00001,
                    _ => throw new ArgumentException("invalid operator")
                }
            };
          
            return res;
        }

        public async Task<GenerateExpressionResponse>  GenerateExpressions(int difficulty)
        {
            var response = new GenerateExpressionResponse();
            (response.Expression1, response.Expression2 ) = 
             difficulty switch
            {
                1 => (await GenerateSimpleNumber(), await GenerateSimpleNumber()),
                2 => (await GenerateSimpleExpression(), await GenerateSimpleExpression()),
                3 => (await GenerateComplexExpression(), await GenerateComplexExpression()),
                _ => throw new ArgumentException("invalid difficulty")
            };
            return response;
        }

        public async Task<string> GenerateSimpleNumber()
        {
            return await Task.FromResult(random.Next(1,100).ToString());
        }

        public async Task<string> GenerateSimpleExpression()
        {
            var leftvalue = await Task.FromResult(random.Next(1, 100));
            var rightvalue = await Task.FromResult(random.Next(1, 100));
            var opr = new[] { "+", "-", "*", "/" };

            var randomOpr = opr[random.Next(opr.Length)];
            return $"{leftvalue} {randomOpr} {rightvalue}";
        }

        public async Task<string> GenerateComplexExpression()
        {
            var expression = await GenerateSimpleExpression();
            var additionalexpression = await GenerateSimpleExpression();
            return $"{expression} {new[] { "*", "/", "-", "+" }[random.Next(3)]} ({additionalexpression}) ";
        }

        private static Task<double> EvaluateExpression(string expression)
        {
            try
            {
                double data = new MathParser().Parse(expression);
                return Task.FromResult(Convert.ToDouble(data));
            }
            catch
            {
                throw new InvalidOperationException("Invalid mathematical expression");
            }
        }
    }
}
