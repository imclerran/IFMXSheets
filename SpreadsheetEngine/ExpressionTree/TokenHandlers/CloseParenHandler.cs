using System;
using System.Collections.Generic;

namespace CptS321
{
    internal class CloseParenHandler : ITokenHandler
    {
        /// <inheritdoc />
        public bool HandlePostfixConversion(string token, ref Stack<string> operatorStack,
            ref List<string> postfixTokens, ref bool encounteredError) {
            if (token != ")") return false;

            string nextOp = "";
            while (operatorStack.Count > 0) {
                nextOp = operatorStack.Pop();
                if (nextOp == "(") {
                    break; // when right paren found, pop from operator stack until left paren
                }
                else {
                    postfixTokens.Add(nextOp); // add non paren operators to postfix expression
                }
            }
            if (nextOp != "(")
                encounteredError = true;

            return true;
        }

        /// <inheritdoc />
        public bool HandleTreeGeneration(string token, ref Stack<ExpNode> nodeStack,
            ref Dictionary<string, double> variables, ref bool encounteredError) {
            if (token != ")") return false;
            encounteredError = true;
            return true;
        }

        /// <inheritdoc />
        public bool HandleTreeEvaluation(ExpNode node, Dictionary<string, double> variables, ref double value) {
            throw new NotImplementedException();
        }
    }
}
