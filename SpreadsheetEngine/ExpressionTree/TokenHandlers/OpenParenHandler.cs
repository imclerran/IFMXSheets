using System;
using System.Collections.Generic;

namespace CptS321
{
    class OpenParenHandler : ITokenHandler
    {
        /// <inheritdoc />
        public bool HandlePostfixConversion(string token, ref Stack<string> operatorStack,
            ref List<string> postfixTokens, ref bool encounteredError) {
            if (token != "(") return false;
            operatorStack.Push(token);
            return true;
        }

        /// <inheritdoc />
        public bool HandleTreeGeneration(string token, ref Stack<ExpNode> nodeStack,
            ref Dictionary<string, double> variables, ref bool encounteredError) {
            if (token != "(") return false;
            encounteredError = true;
            return true;
        }

        /// <inheritdoc />
        public bool HandleTreeEvaluation(ExpNode node, Dictionary<string, double> variables, ref double value) {
            throw new NotImplementedException();
        }
    }
}
