using System;
using System.Collections.Generic;

namespace CptS321
{
    class UnknownTokenHandler : ITokenHandler
    {
        /// <inheritdoc />
        public bool HandlePostfixConversion(string token, ref Stack<string> operatorStack,
            ref List<string> postfixTokens, ref bool encounteredError) {
            if ("" != token)
                encounteredError = true;
            return true; // can handle all operators
        }

        /// <inheritdoc />
        public bool HandleTreeGeneration(string token, ref Stack<ExpNode> nodeStack,
            ref Dictionary<string, double> variables, ref bool encounteredError) {
            encounteredError = true;
            return true; // can handle all operators
        }

        /// <inheritdoc />
        public bool HandleTreeEvaluation(ExpNode node, Dictionary<string, double> variables, ref double value) {
            throw new NotImplementedException();
        }
    }
}
