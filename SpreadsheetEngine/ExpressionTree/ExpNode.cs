using System.Collections.Generic;

namespace CptS321
{
    /// <summary>
    /// binary node for expression tree
    /// </summary>
    internal class ExpNode
    {
        #region fields

        public ExpNode LHS;
        public ExpNode RHS;

        #endregion

        #region properties

        public string Token { get; }

        #endregion

        #region constructor

        public ExpNode(string token) {
            Token = token;
        }

        #endregion

        #region methods

        /// <summary>
        /// recursively evaluate the value of an ExpNode
        /// </summary>
        /// <param name="variables">a dictionary of variable names and values</param>
        /// <returns>the double value of the evaluated ExpNode</returns>
        internal double Evaluate(Dictionary<string, double> variables) {
            double value = 0.0d;
            var tokenHandler = TokenHandlerFactory.GenerateTokenHandler(Token);
            tokenHandler.HandleTreeEvaluation(this, variables, ref value);
            return value;
        }

        #endregion
    }
}