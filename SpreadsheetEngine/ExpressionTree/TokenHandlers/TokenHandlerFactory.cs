using System.Collections.Generic;
using System.Text.RegularExpressions;
using SpreadsheetEngine.ExpressionTree.TokenHandlers;

namespace CptS321
{
    internal class TokenHandlerFactory
    {
        #region fields

        private static readonly string RXopenParen = @"^([(])$";
        private static readonly string RXcloseParen = @"^([)])$";
        private static readonly string RXnonParenOperators = @"^([+*/^]|[-])$";

        private static readonly string RXvariable = @"^([a-zA-Z]+([0-9]*[a-zA-Z]*)*)$";
        private static readonly string RXnumber = @"^([0-9]+[" + Regex.Escape(".") + "]?[0-9]*)$";
        private static readonly string RXfunctions = @"^(SIN|COS|LOG)$";
        private static readonly string RXconstant = @"^(pi|e)$";

        #endregion

        #region methods

        /// <summary>
        /// create a token handler to handle converting the given token to a postfix expression
        /// </summary>
        /// <param name="token"></param>
        /// <returns>a token handler for the given token, or null if no appropriate handler</returns>
        public static ITokenHandler GenerateTokenHandler(string token) {
            if (Regex.IsMatch(token, RXfunctions))
                return new FunctionHandler();
            if (Regex.IsMatch(token, RXconstant))
                return new ConstantHandler();
            if (Regex.IsMatch(token, RXnumber))
                return new NumberHandler();
            if (Regex.IsMatch(token, RXvariable))
                return new VariableHandler();
            if (Regex.IsMatch(token, RXopenParen))
                return new OpenParenHandler();
            if (Regex.IsMatch(token, RXcloseParen))
                return new CloseParenHandler();
            if (Regex.IsMatch(token, RXnonParenOperators))
                return new NonParenOperatorHandler();
            if (token == ",")
                return new CommaHandler();

            return new UnknownTokenHandler();
        }

        /// <summary>
        /// generate an ordered list of token handlers
        /// </summary>
        /// <returns>an ordered list of token handlers</returns>
        public static List<ITokenHandler> GenerateTokenHandlerList() {
            var tl = new List<ITokenHandler>();
            tl.Add(new FunctionHandler());
            tl.Add(new ConstantHandler());
            tl.Add(new NumberHandler());
            tl.Add(new VariableHandler());
            tl.Add(new OpenParenHandler());
            tl.Add(new CloseParenHandler());
            tl.Add(new NonParenOperatorHandler());
            tl.Add(new CommaHandler());
            tl.Add(new UnknownTokenHandler());
            return tl;
        }

        #endregion
    }
}
