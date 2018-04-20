using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CptS321
{
    internal class ConstantHandler : ITokenHandler
    {
        private readonly string RXconstant = @"^(pi|e)$";

        /// <inheritdoc />
        public bool HandlePostfixConversion(string token, ref Stack<string> operatorStack, ref List<string> postfixTokens, ref bool encounteredError) {
            if (!Regex.IsMatch(token, RXconstant)) return false;
            postfixTokens.Add(token);
            return true;
        }

        /// <inheritdoc />
        public bool HandleTreeGeneration(string token, ref Stack<ExpNode> nodeStack, ref Dictionary<string, double> variables, ref bool encounteredError) {
            if (!Regex.IsMatch(token, RXconstant)) return false;
            nodeStack.Push(new ExpNode(token));
            return true;
        }

        /// <inheritdoc />
        public bool HandleTreeEvaluation(ExpNode node, Dictionary<string, double> variables, ref double value) {
            if (!Regex.IsMatch(node.Token, RXconstant)) return false;
            switch (node.Token) {
                case "pi":
                    value = Math.PI;
                    break;
                case "e":
                    value = Math.E;
                    break;
                default:
                    value = 0.0d;
                    break;
            }
            return true;
        }
    }
}