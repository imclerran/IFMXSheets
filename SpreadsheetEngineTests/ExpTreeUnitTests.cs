using CptS321;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

// TODO: Need unit tests for Spreadsheet class

namespace SpreadsheetEngineTests
{
    [TestClass]
    public class ExpTreeTest
    {
        [TestMethod]
        public void TestEmptyStringExpression() {
            const string testExpression = "";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestEmptyStringExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be false");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 0");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsFalse(isValidExpression);
            Assert.AreEqual(value, 0.0d);
        }

        [TestMethod]
        public void TestNonsenseStringExpression() {
            const string testExpression = "Hello, World!";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestNonsenseStringExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be false");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 0");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsFalse(isValidExpression);
            Assert.AreEqual(value, 0.0d);
        }

        [TestMethod]
        public void TestIncompleteOperatorExpression() {
            const string testExpression = "1+";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestNonsenseStringExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be false");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 0");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsFalse(isValidExpression);
            Assert.AreEqual(value, 0.0d);
        }

        [TestMethod]
        public void TestSingleConstantExpression() {
            const string testExpression = "pi";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestSingleConstantExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be true");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 3.14159265358979");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsTrue(isValidExpression);
            Assert.AreEqual(value, Math.PI);
        }

        [TestMethod]
        public void TestSingleVariableExpression() {
            const string testVarName = "a1";
            const double testVarValue = 123.45d;
            const string testExpression = testVarName;
            var testTree = new ExpTree(testExpression);
            testTree.SetVar(testVarName, testVarValue);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestSingleVariableExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be true");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 123.45");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsTrue(isValidExpression);
            Assert.AreEqual(value, testVarValue);
        }

        [TestMethod]
        public void TestMultivariableExpression() {
            const string testVarName1 = "a1";
            const string testVarName2 = "Z50";
            const double testVarValue1 = 123.45d;
            const double testVarValue2 = double.MaxValue;
            const string testExpression = "a1-Z50";
            var testTree = new ExpTree(testExpression);
            testTree.SetVar(testVarName1, testVarValue1);
            testTree.SetVar(testVarName2, testVarValue2);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Assert.IsTrue(isValidExpression);
            Assert.AreEqual(value, (testVarValue1 - testVarValue2));
            Console.WriteLine("TestMultiVariableExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be true");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: -1.79769313486232E+308");
            Console.WriteLine("  Actual value:  {0}", value);
        }

        [TestMethod]
        public void TestPlusExpression() {
            const string testExpression = "1+1";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestPlusExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be true");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 2");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsTrue(isValidExpression);
            Assert.AreEqual(value, (1 + 1));
        }

        [TestMethod]
        public void TestMinusExpression() {
            const string testExpression = "1-2";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestMinusExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be true");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: -1");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsTrue(isValidExpression);
            Assert.AreEqual(value, (1 - 2));
        }

        [TestMethod]
        public void TestMultiplyExpression() {
            const string testExpression = "2*3";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestMultiplyExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be true");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 6");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsTrue(isValidExpression);
            Assert.AreEqual(value, (2 * 3));
        }

        [TestMethod]
        public void TestDivideExpression() {
            const string testExpression = "3/2";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestDividExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be true");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 1.5");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsTrue(isValidExpression);
            Assert.AreEqual(value, (3d / 2d));
        }

        [TestMethod]
        public void TestExponentiateExpression() {
            const string testExpression = "2^4";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestExponentiateExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be true");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 16");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsTrue(isValidExpression);
            Assert.AreEqual(value, (Math.Pow(2, 4)));
        }

        [TestMethod]
        public void TestParentheseExpression() {
            const string testExpression = "(2+2)^(4/2)";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestParentheseExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be true");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 16");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsTrue(isValidExpression);
            Assert.AreEqual(value, Math.Pow((2 + 2), (4d / 2d)));
        }

        [TestMethod]
        public void TestUnclosedParenExpression() {
            const string testExpression = "(1+1";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestNonsenseStringExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be false");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 0");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsFalse(isValidExpression);
            Assert.AreEqual(value, 0.0d);
        }

        [TestMethod]
        public void TestUnopenedParenExpression() {
            const string testExpression = "1+1)";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestNonsenseStringExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be false");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 0");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsFalse(isValidExpression);
            Assert.AreEqual(value, 0.0d);
        }

        [TestMethod]
        public void TestWhitespaceSeparatedExpression() {
            const string testExpression = " 1 + 1 ";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestWhitespaceSeparatedExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be true");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 2");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsTrue(isValidExpression);
            Assert.AreEqual(value, (1 + 1));
        }

        [TestMethod]
        public void TestSineFunctionExpression() {
            const string testExpression = "SIN(1+1)";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestSineFunctionExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be true");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 0.909297426825682");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsTrue(isValidExpression);
            Assert.AreEqual(value, (Math.Sin(1 + 1)));
        }

        [TestMethod]
        public void TestCosineFunctionExpression() {
            const string testExpression = "COS(2)*2";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestCosineFunctionExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be true");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: -0.832293673094285");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsTrue(isValidExpression);
            Assert.AreEqual(value, (Math.Cos(2) * 2));
        }

        [TestMethod]
        public void TestLogFunctionExpression() {
            const string testExpression = "LOG(10^2,10)";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestLogFunctionExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be true");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 2");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsTrue(isValidExpression);
            Assert.AreEqual(value, (Math.Log(100, 5 + 5)));
        }

        [TestMethod]
        public void TestSineNoArgsExpression() {
            const string testExpression = "SIN()";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestNonsenseStringExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be false");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 0");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsFalse(isValidExpression);
            Assert.AreEqual(value, 0.0d);
        }

        [TestMethod]
        public void TestCosineExtraArgsExpression() {
            const string testExpression = "COS(10,2)";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestNonsenseStringExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be false");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 0");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsFalse(isValidExpression);
            Assert.AreEqual(value, 0.0d);
        }

        [TestMethod]
        public void TestLogIncompleteExpression() {
            const string testExpression = "LOG(10,)";
            var testTree = new ExpTree(testExpression);
            var isValidExpression = testTree.IsValidExpression();
            var value = testTree.Eval();
            Console.WriteLine("TestNonsenseStringExpression:");
            Console.WriteLine(" ExpTree.IsValidExpression() should be false");
            Console.WriteLine("  IsValidExpression() returned {0}", isValidExpression);
            Console.WriteLine(" Expected value: 0");
            Console.WriteLine("  Actual value:  {0}", value);
            Assert.IsFalse(isValidExpression);
            Assert.AreEqual(value, 0.0d);
        }
    }
}