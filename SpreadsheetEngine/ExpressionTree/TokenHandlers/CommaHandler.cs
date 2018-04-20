using System;
using System.Collections.Generic;
using System.Text;
using CptS321;

namespace SpreadsheetEngine.ExpressionTree.TokenHandlers
{
    internal class CommaHandler : ITokenHandler
    {
        /// <inheritdoc />
        public bool HandlePostfixConversion(string token, ref Stack<string> operatorStack, ref List<string> postfixTokens, ref bool encounteredError) {
            if (token != ",") return false;
            while (operatorStack.Count > 0) {
                string nextOp = operatorStack.Pop();
                if (nextOp == "(") {
                    operatorStack.Push(nextOp);
                    break; // when right paren found, pop from operator stack until left paren
                }
                else {
                    postfixTokens.Add(nextOp); // add non paren operators to postfix expression
                }
            }
            return true;
        }

        /// <inheritdoc />
        public bool HandleTreeGeneration(string token, ref Stack<ExpNode> nodeStack, ref Dictionary<string, double> variables, ref bool encounteredError) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool HandleTreeEvaluation(ExpNode node, Dictionary<string, double> variables, ref double value) {
            throw new NotImplementedException();
        }
    }
}
