using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CptS321
{
    internal class NumberHandler : ITokenHandler
    {
        private readonly string RXnumber = @"^([0-9]+[" + Regex.Escape(".") + "]?[0-9]*)$";

        /// <inheritdoc />
        public bool HandlePostfixConversion(string token, ref Stack<string> operatorStack,
            ref List<string> postfixTokens, ref bool encounteredError) {
            if (!Regex.IsMatch(token, RXnumber)) return false;
            postfixTokens.Add(token);
            return true;
        }

        /// <inheritdoc />
        public bool HandleTreeGeneration(string token, ref Stack<ExpNode> nodeStack, ref Dictionary<string, double> variables, ref bool encounteredError) {
            if (!Regex.IsMatch(token, RXnumber)) return false;
            nodeStack.Push(new ExpNode(token));
            return true;
        }

        /// <inheritdoc />
        public bool HandleTreeEvaluation(ExpNode node, Dictionary<string, double> variables, ref double value) {
            if (!Regex.IsMatch(node.Token, RXnumber)) return false;
            value = double.Parse(node.Token);
            return true;
        }
    }
}
