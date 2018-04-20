using System.Collections.Generic;

namespace CptS321
{
    /// <summary>
    /// Interface for handling postfix conversion, expression tree generation, and expression tree evaluation
    /// for any token found in an expression
    /// </summary>
    internal interface ITokenHandler
    {
        /// <summary>
        /// handle the conversion of an expression token from infix to postfix
        /// </summary>
        /// <param name="token">the token to handle</param>
        /// <param name="operatorStack">a stack of operators used in conversion</param>
        /// <param name="postfixTokens">a list of tokens representing the postfix expression</param>
        /// <param name="encounteredError"> boolean flag used to indicate an error during tree generation</param>
        /// <returns>returns true if the handler was able to handle the token</returns>
        bool HandlePostfixConversion(string token, ref Stack<string> operatorStack, 
            ref List<string> postfixTokens, ref bool encounteredError);

        /// <summary>
        /// handle the generation of an expression tree for a given token
        /// </summary>
        /// <param name="token">the token to handle</param>
        /// <param name="nodeStack">a stack of ExpNodes used in tree generation</param>
        /// <param name="variables">a dictionary of variables contained in the expression tree</param>
        /// <param name="encounteredError">a boolean flag used to indicate an error during tree generation</param>
        /// <returns>returns true if the handler was able to handle the token</returns>
        bool HandleTreeGeneration(string token, ref Stack<ExpNode> nodeStack, ref Dictionary<string, double> variables, ref bool encounteredError);

        /// <summary>
        /// handle the evaluation of a token within an expression tree
        /// </summary>
        /// <param name="node">the expression tree node containing the token to evaluate</param>
        /// <param name="variables">a dictionary of variables contained in the expression tree</param>
        /// <param name="value">the value determined for the token in the expression tree</param>
        /// <returns>returns true if the handler was able to handle the token</returns>
        bool HandleTreeEvaluation(ExpNode node, Dictionary<string, double> variables, ref double value);
    }
}
