using System;
using CptS321;

namespace ExpTreeConsole
{
    class Program
    {
        #region fields

        private static string _expStr;
        private static ExpTree _expTree;

        #endregion

        #region main

        static void Main(string[] args)
        {
            _expStr = "none";
            _expTree = new ExpTree(_expStr);

            do
            {
                ShowMenu();
                string line = Console.ReadLine();
                switch (line)
                {
                    case "1":
                        Menu1();
                        break;
                    case "2":
                        Menu2();
                        break;
                    case "3":
                        Menu3();
                        break;
                    case "4":
                        return;
                    default:
                        //Console.WriteLine("Invalid entry");
                        break;
                }
            } while (true);
        }

        #endregion

        #region methods

        //=====================================================================
        // ShowMenu() -- show the main menu
        //=====================================================================
        private static void ShowMenu()
        {
            Console.WriteLine("Menu (Current expression: {0})", _expStr);
            Console.WriteLine(" 1) Enter a new expression");
            Console.WriteLine(" 2) Set a variable value");
            Console.WriteLine(" 3) Evaluate tree");
            Console.WriteLine(" 4) Quit");
        }

        //=====================================================================
        // Menu1() -- get new expression from user
        //=====================================================================
        private static void Menu1()
        {
            Console.Write("Enter a new expression: ");
            _expStr = Console.ReadLine();
            _expTree = new ExpTree(_expStr);
        }

        //=====================================================================
        // Menu2() -- get a variable name/value pair from user
        //=====================================================================
        private static void Menu2()
        {
            Console.Write("Enter a variable name: ");
            string name = Console.ReadLine();
            Console.Write("Enter a variable value: ");
            string valstr = Console.ReadLine();
            if (double.TryParse(valstr, out var val))
            {
                _expTree.SetVar(name, val);
            }
            else
            {
                Console.WriteLine("Could not interpret value");
            }
        }

        //=====================================================================
        // Menu3() -- evaluate the expression tree
        //=====================================================================
        private static void Menu3()
        {
            if (null != _expTree && null != _expStr)
            {
                if (_expTree.IsValidExpression())
                {
                    string result = _expTree.Eval().ToString();
                    //Console.WriteLine(_expStr + " = " + result);
                    Console.WriteLine(result);
                }
                else
                    Console.WriteLine("Invalid expression");
            }
            else
            {
                Console.WriteLine("No expresion entered");
            }
        }

        #endregion
    }
}
