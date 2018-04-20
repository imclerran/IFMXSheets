using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CptS321
{
    internal class VariableHandler : ITokenHandler
    {
        private readonly string RXvariable = @"^([a-zA-Z]+([0-9]*[a-zA-Z]*)*)$";

        /// <inheritdoc />
        public bool HandlePostfixConversion(string token, ref Stack<string> operatorStack,
            ref List<string> postfixTokens, ref bool encounteredError) {
            if (!Regex.IsMatch(token, RXvariable)) return false;
            postfixTokens.Add(token);
            return true;
        }

        /// <inheritdoc />
        public bool HandleTreeGeneration(string token, ref Stack<ExpNode> nodeStack, ref Dictionary<string, double> variables, ref bool encounteredError) {
            if (!Regex.IsMatch(token, RXvariable)) return false;
            variables[token] = 0.0f;
            nodeStack.Push(new ExpNode(token));
            return true;
        }

        /// <inheritdoc />
        public bool HandleTreeEvaluation(ExpNode node, Dictionary<string, double> variables, ref double value) {
            if (!Regex.IsMatch(node.Token, RXvariable)) return false;
            value = variables.ContainsKey(node.Token) ? variables[node.Token] : 0.0f;
            return true;
        }
    }
}
