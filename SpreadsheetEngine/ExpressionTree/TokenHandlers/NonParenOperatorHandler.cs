using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CptS321
{
    class NonParenOperatorHandler : ITokenHandler
    {
        private static readonly string RXnonParenOperators = @"^([+*/^]|[-])$";

        /// <inheritdoc />
        public bool HandlePostfixConversion(string token, ref Stack<string> operatorStack,
            ref List<string> postfixTokens, ref bool encounteredError) {
            if (!Regex.IsMatch(token, RXnonParenOperators)) return false;
            while (operatorStack.Count > 0) {
                string nextOp = operatorStack.Pop();
                if (nextOp == "(" || (GetPrecedence(nextOp) < GetPrecedence(token))) {
                    operatorStack.Push(nextOp);
                    break; // pop operators from stack until paren or operator of lower precedence found
                }
                else {
                    postfixTokens.Add(nextOp); // add popped operators of higher precedence to postfix expression
                }
            }
            operatorStack.Push(token); // after high precedence operators popped, push new operator to stack
            return true;
        }

        /// <inheritdoc />
        public bool HandleTreeGeneration(string token, ref Stack<ExpNode> nodeStack, ref Dictionary<string, double> variables, ref bool encounteredError) {
            if (!Regex.IsMatch(token, RXnonParenOperators)) return false;

            ExpNode tmp = new ExpNode(token);
            if (nodeStack.Count >= 2) {
                // when operator found, if there are at least two nodes
                // on the stack, pop the top two and add them as as right
                // and left nodes for the operator, then push operator to
                // the node stack
                tmp.RHS = nodeStack.Pop();
                tmp.LHS = nodeStack.Pop();
                nodeStack.Push(tmp);
            }
            else { // not enough nodes in stack to assign to right/left of operator
                encounteredError = true;
            }
            return true;
        }

        /// <inheritdoc />
        public bool HandleTreeEvaluation(ExpNode node, Dictionary<string, double> variables, ref double value) {
            if (!Regex.IsMatch(node.Token, RXnonParenOperators)) return false;
            switch (node.Token) {
                case "+":
                    value = node.LHS.Evaluate(variables) + node.RHS.Evaluate(variables);
                    break;
                case "-":
                    value = node.LHS.Evaluate(variables) - node.RHS.Evaluate(variables);
                    break;
                case "*":
                    value = node.LHS.Evaluate(variables) * node.RHS.Evaluate(variables);
                    break;
                case "/":
                    value = node.LHS.Evaluate(variables) / node.RHS.Evaluate(variables);
                    break;
                case "^":
                    value = Math.Pow(node.LHS.Evaluate(variables), node.RHS.Evaluate(variables));
                    break;
                default:
                    value = 0.0f;
                    break;
            }
            return true;
        }

        /// <summary>
        /// get the precedence of a given operator
        /// </summary>
        /// <param name="operator">a string representing the operator</param>
        /// <returns>the precedence of the operator from 0 to 3, or -1 if unknown</returns>
        private static int GetPrecedence(string @operator) {
            switch (@operator) {
                case "SIN": return 3;
                case "COS": return 3;
                case "LOG": return 3;
                case "(": return 3;
                case ")": return 3;
                case "^": return 2;
                case "*": return 1;
                case "/": return 1;
                case "+": return 0;
                case "-": return 0;
                default: return -1;
            }
        }
    }
}
