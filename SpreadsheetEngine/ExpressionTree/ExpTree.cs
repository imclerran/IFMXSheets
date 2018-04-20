using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CptS321
{
    /// <summary>
    /// class generates an expression tree based on a mathmatical
    /// equation contained in a string, and allows evaluation of
    /// the equation, including any variables in the expression
    /// </summary>
    public class ExpTree
    {
        #region fields

        public Dictionary<string, double> Variables;
        private List<string> _postfixTokens;
        private readonly List<ITokenHandler> _tokenHandlers;
        private ExpNode _root;
        private bool _encounteredError;
        private readonly string RXoperatorCapture = @"([+*/^(),]|[-])";

        #endregion

        #region constructor

        /// <summary>
        /// initialize variables, convert infix expression
        /// to postfix, then generate expression tree
        /// </summary>
        /// <param name="expression">a string containing an infix expression to convert to an expression tree</param>
        public ExpTree(string expression) {
            Variables = new Dictionary<string, double>();
            _postfixTokens = new List<string>();
            _tokenHandlers = TokenHandlerFactory.GenerateTokenHandlerList();
            _encounteredError = false;
            If2Pf(expression);
            GenerateTree();
        }

        #endregion

        #region methods

        /// <summary>
        /// convert an infix expression string to a tokenized postfix expression
        /// </summary>
        /// <param name="infixStr">a string containing an infix expression</param>
        private void If2Pf(string infixStr) {
            Stack<string> operatorStack = new Stack<string>();
            string[] infixTokens = Regex.Split(infixStr, RXoperatorCapture);

            for (int i = 0; i < infixTokens.Length; i++) {
                infixTokens[i] = infixTokens[i].Trim();
            }

            foreach (var token in infixTokens) {
                int i = 0;
                while (!_tokenHandlers[i].HandlePostfixConversion(token, ref operatorStack, ref _postfixTokens, ref _encounteredError)) i++;
            }

            while (operatorStack.Count > 0) {
                _postfixTokens.Add(operatorStack.Pop()); // pop remaining operators and add to postfix expression
            }
        }

        /// <summary>
        /// generate expression tree from postfix expression
        /// </summary>
        private void GenerateTree() {
            Stack<ExpNode> nodeStack = new Stack<ExpNode>();
            foreach (var token in _postfixTokens) {
                int i = 0;
                while (!_tokenHandlers[i].HandleTreeGeneration(token, ref nodeStack, ref Variables, ref _encounteredError)) i++;
                if (_encounteredError) {
                    _root = null;
                    return;
                }
            }
            // should be exactly one node in stack after completing tree
            // if there is not exactly one node, set root as null to indicate error
            _root = nodeStack.Count == 1 ? nodeStack.Pop() : null;
        }

        /// <summary>
        /// assign a value to a variable in the Variables dictionary
        /// </summary>
        /// <param name="varName">the name of the variable to store</param>
        /// <param name="varValue">the value of the variable to store</param>
        public void SetVar(string varName, double varValue) {
            Variables[varName] = varValue;
        }

        /// <summary>
        /// get a string representation of the postfix expression describe by the tree
        /// </summary>
        /// <returns>the postfix expression string</returns>
        public string GetPostfix() {
            string pf = "";
            foreach (var token in _postfixTokens) {
                pf += token + " ";
            }
            return pf;
        }

        /// <summary>
        /// check if tree has been initialized with a valid expression
        /// </summary>
        /// <returns>boolean indicating if the expression tree is valid</returns>
        public bool IsValidExpression() {
            return _root != null && !_encounteredError ? true : false; // tree is valid if root not null and no error encountered
        }

        /// <summary>
        /// evaluate expression tree, or return 0 if tree invalid
        /// </summary>
        /// <returns>the double value of the evaluated expression tree</returns>
        public double Eval() {
            return _root?.Evaluate(Variables) ?? 0.0f;
        }

        #endregion
    }
}
