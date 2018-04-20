using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CptS321
{
    internal class FunctionHandler : ITokenHandler
    {
        private static readonly string RXfunctions = @"^(SIN|COS|LOG)$";

        /// <inheritdoc />
        public bool HandlePostfixConversion(string token, ref Stack<string> operatorStack,
            ref List<string> postfixTokens, ref bool encounteredError) {
            if (!Regex.IsMatch(token, RXfunctions)) return false;
            operatorStack.Push(token);
            return true;
        }

        /// <inheritdoc />
        public bool HandleTreeGeneration(string token, ref Stack<ExpNode> nodeStack,
            ref Dictionary<string, double> variables, ref bool encounteredError) {
            if (!Regex.IsMatch(token, RXfunctions)) return false;
            var node = new ExpNode(token);
            if (token == "LOG") {
                if (nodeStack.Count >= 2) {
                    node.RHS = nodeStack.Pop();
                    node.LHS = nodeStack.Pop();
                }
                else {
                    encounteredError = true;
                }
            }
            else {
                if (nodeStack.Count >= 1) {
                    node.LHS = nodeStack.Pop();
                }
                else {
                    encounteredError = true;
                }
            }
            nodeStack.Push(node);
            return true;
        }

        /// <inheritdoc />
        public bool HandleTreeEvaluation(ExpNode node, Dictionary<string, double> variables, ref double value) {
            if (!Regex.IsMatch(node.Token, RXfunctions)) return false;
            switch (node.Token) {
                case "SIN":
                    value = Math.Sin(node.LHS.Evaluate(variables));
                    break;
                case "COS":
                    value = Math.Cos(node.LHS.Evaluate(variables));
                    break;
                case "LOG":
                    value = Math.Log(node.LHS.Evaluate(variables), node.RHS.Evaluate(variables));
                    break;
                default:
                    value = 0.0f;
                    break;
            }
            return true;
        }
    }
}
