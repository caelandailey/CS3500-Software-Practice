using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System.Collections.Generic;

/// <summary>
/// The class tests all the functions in the Formula class. Creates to normalizers and validators that are used in the Formula's constructor,
/// as well as a lookup method which is necessay for the Evaluate method.
/// 
/// Author: Karina Biancone
/// </summary>

namespace FormulaTester
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// An empty formula should throw a formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void emptyTest()
        {
            Formula empty = new Formula("");

        }

        /// <summary>
        /// Begins formula with an operator, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void improperFirstToken1()
        {
            Formula k = new Formula("+ 2 + 8");
        }

        /// <summary>
        /// Begins formula with a closing parenthesis, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void improperFirstToken2()
        {
            Formula k = new Formula(") 2 + 8");
        }

        /// <summary>
        /// Begins formula with an invalid variable, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void improperFirstToken3()
        {
            Formula k = new Formula("x! - 2 + 8");
        }

        /// <summary>
        /// '=6' will be read as a single token which is invalid, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void improperFirstToken4()
        {
            Formula k = new Formula("= 6 +2-8");
        }

        /// <summary>
        /// '8?' will be read as a single token which is invalid, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void invalidToken1()
        {
            Formula k = new Formula("6+2-8?");
        }

        /// <summary>
        /// '{3' will be read as a single token which is invalid, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void invalidToken2()
        {
            Formula k = new Formula("6+2 + {3 - 8");
        }

        /// <summary>
        /// '@^' will be read as a single token which is invalid, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void invalidToken3()
        {
            Formula k = new Formula("6 @^ + 9");
        }


        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void invalidVariable1()
        {
            Formula k = new Formula("XJ_?");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void invalidVariable2()
        {
            Formula k = new Formula("f#");
        }

        /// <summary>
        /// More open parenthesis than closed, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void parenthesisBalance1()
        {
            Formula k = new Formula("7 + ((8 - 2 + 6)");
        }

        /// <summary>
        /// More closed parenthesis than open, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void parenthesisBalance2()
        {
            Formula k = new Formula("6 - 3 + (3*1) + 3)");
        }

        /// <summary>
        /// More open parenthesis than closed, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void parenthesisBalance3()
        {
            Formula k = new Formula("(7 + (1) - 3 + 8");
        }

        /// <summary>
        /// Closed parenthesis before double, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeNumberError1()
        {
            Formula k = new Formula("(7 * 2) 3");
        }

        /// <summary>
        /// Two doubles next to eachother, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeNumberError2()
        {
            Formula k = new Formula("7 * 2 3");
        }

        /// <summary>
        /// Variable and double next to eachother, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeNumberError3()
        {
            Formula k = new Formula("(7 * 3) + x4 3");
        }

        /// <summary>
        /// Closed parenthesis before variable, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeVariableError1()
        {
            Formula k = new Formula("(7 * 2) + (6) x4");
        }

        /// <summary>
        /// Double before variable, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeVariableError2()
        {
            Formula k = new Formula("(7 * 2) + 6 x4");
        }

        /// <summary>
        /// Two variables next to one another, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeVariableError3()
        {
            Formula k = new Formula("(7 * 2) + y4 x4");
        }

        /// <summary>
        /// Closed parenthesis before an open parenthesis, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeLParenthesisError1()
        {
            Formula k = new Formula("(7 * 2)(3 - 4)");
        }

        /// <summary>
        /// Double before open parenthesis, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeLParenthesisError2()
        {
            Formula k = new Formula("7(3 - 4)");
        }

        /// <summary>
        /// Variable before open parenthesis, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeLParenthesisError3()
        {
            Formula k = new Formula("(3 - 4) + x3 (2 + 3)");
        }

        /// <summary>
        /// Operator before closed parenthesis, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeRParenthesisError1()
        {
            Formula k = new Formula("(3 + 2) - (+)");
        }

        /// <summary>
        /// Open and closed parenthesis next to eachother, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeRParenthesisError2()
        {
            Formula k = new Formula("3 + ()");
        }

        /// <summary>
        /// Open parenthesis before operator, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeOperator1()
        {
            Formula k = new Formula("( 3 + 2) - (* 2)");
        }

        /// <summary>
        /// Two operators next eachother, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeOperator2()
        {
            Formula k = new Formula("( 3 + 2) + x3 - - 2)");
        }

        /// <summary>
        /// Last token is an operator, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void lastToken1()
        {
            Formula k = new Formula("6 + 2 -");
        }

        /// <summary>
        /// Last token is an invalid variable.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void lastToken2()
        {
            Formula k = new Formula("6 + 2 - 3x");
        }

        /// <summary>
        /// Last token is open parenthesis, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void lastToken3()
        {
            Formula k = new Formula("6 + 2 - (");
        }

        /// <summary>
        /// Performs a simple formula.
        /// </summary>
        [TestMethod]
        public void simpleEvaluate1()
        {
            Formula k = new Formula("6 + 2 - 6");
            Assert.AreEqual(2.0, k.Evaluate(lookup));
        }

        /// <summary>
        /// Performs a simple formula that is in parenthesis
        /// </summary>
        [TestMethod]
        public void simpleEvaluate2()
        {
            Formula k = new Formula("( 6 + 2 - 6 )");
            Assert.AreEqual(2.0, k.Evaluate(lookup));
        }

        /// <summary>
        /// Has a number without any operations in parenthesis
        /// </summary>
        [TestMethod]
        public void simpleEvaluate3()
        {
            Formula k = new Formula("( 2 )");
            Assert.AreEqual(2.0, k.Evaluate(lookup));
        }

        /// <summary>
        /// Makes sure that the Evaluator can calculate doubles
        /// </summary>
        [TestMethod]
        public void simpleEvaluate4()
        {
            Formula k = new Formula("( 6 + 3.01 )");
            Assert.AreEqual(9.01, k.Evaluate(lookup));
        }

        /// <summary>
        /// A formula with all division and multiplication
        /// </summary>
        [TestMethod]
        public void simpleEvaluate5()
        {
            Formula k = new Formula("6 * 6 / 6");
            Assert.AreEqual(6.0, k.Evaluate(lookup));
        }

        /// <summary>
        /// A formula with all division and multiplication in parenthesis
        /// </summary>
        [TestMethod]
        public void simpleEvaluate6()
        {
            Formula k = new Formula("(6 * 6 / 6)");
            Assert.AreEqual(6.0, k.Evaluate(lookup));
        }

        /// <summary>
        /// Addition in parenthesis with multiplication outside
        /// </summary>
        [TestMethod]
        public void parenthEvaluate1()
        {
            Formula k = new Formula("(2+6)*3");
            Assert.AreEqual(24.0, k.Evaluate(lookup));
        }

        /// <summary>
        /// A formula only using variables embedded in parenthesis
        /// </summary>
        [TestMethod]
        public void parenthEvaluate2()
        {
            double error = .00001;
            Formula k = new Formula("((((a6+x9)+pX)+a6)+A6)+1");
            Assert.AreEqual((double)16.604, (double)k.Evaluate(lookup), error);
        }

        /// <summary>
        /// An operation in parenthesis in the middle of the formula
        /// </summary>
        [TestMethod]
        public void parenthEvaluate3()
        {
            Assert.AreEqual(194.0, new Formula("2+3*5+(3+4*8)*5+2").Evaluate(lookup));
        }

        /// <summary>
        /// A double in parenthesis
        /// </summary>
        [TestMethod]
        public void parenthEvaluate4()
        {
            Assert.AreEqual((double)4.00043, (double)new Formula("(4.00043)").Evaluate(lookup));
        }

        /// <summary>
        /// A formula using a decimal
        /// </summary>
        [TestMethod]
        public void decimalEvaluate()
        {
            double error = .0001;
            double expected = 5.350009;
            Formula k = new Formula("1.5 * .9 + 4.000009");
            Assert.AreEqual(expected, (double)k.Evaluate(lookup), error);
        }

        /// <summary>
        /// The end of the formula has division.
        /// </summary>
        [TestMethod]
        public void divisionEndingEvaluate1()
        {
            Formula k = new Formula("( 6 + 6 - 4 / 2 )");
            Assert.AreEqual(10.0, k.Evaluate(lookup));
        }

        /// <summary>
        /// Uses a normalizer that converts the variable to uppercase and then checks that lookup is getting
        /// the correct value for that variable (A6 instead of a6... 4 instead of 2)
        /// </summary>
        [TestMethod]
        public void checkNormalizer1()
        {
            Formula k = new Formula("a6 + 2", normalizeUpper, isValidLastDigit);
            Assert.AreEqual(6.0, k.Evaluate(lookup));
        }

        /// <summary>
        /// The normalize function will reverse the variable, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void checkNormalizer2()
        {
            Formula k = new Formula("f4", normalizeReverse, isValidLastDigit);
        }

        /// <summary>
        /// The normalize function will reverse the variable making it invalid after passing through
        /// validation function, throw formula format exception
        /// </summary>
        [TestMethod]
        public void checkNormalizer3()
        {
            Formula k = new Formula("a___", normalizeReverse, isValidNoDigits);
        }

        /// <summary>
        /// The validator looks for a digit at the end of the variables in the formula, throws 
        /// formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void checkValidator1()
        {
            Formula k = new Formula("aa + 44 - ff", normalizeReverse, isValidLastDigit);
        }

        /// <summary>
        /// Validator makes sure there are no digits in the variable, even after normalizer
        /// </summary>
        [TestMethod]
        public void checkValidator2()
        {
            Formula k = new Formula("a___", normalizeReverse, isValidNoDigits);
        }

        /// <summary>
        /// Checks that the correct value is being passed for the specific variable found in the lookup function
        /// </summary>
        [TestMethod]
        public void checkLookup1()
        {
            Formula k = new Formula("a6 + 2");
            Assert.AreEqual(4.0, k.Evaluate(lookup));
        }

        /// <summary>
        /// Passes a variable that is not in the lookup function, results to a formula error
        /// </summary>
        [TestMethod]
        public void checkLookup2()
        {
            Formula k = new Formula("x4 + 2");
            Assert.IsInstanceOfType(k.Evaluate(lookup), typeof(FormulaError));
        }

        /// <summary>
        /// Test a simple division by zero
        /// </summary>
        [TestMethod]
        public void divideByZero1()
        {
            Formula k = new Formula("3/0");
            Assert.IsInstanceOfType(k.Evaluate(lookup), typeof(FormulaError));
        }

        /// <summary>
        /// Formula results to a division by zero if done in proper order, results with a formula errror
        /// </summary>
        [TestMethod]
        public void divideByZero2()
        {
            Formula k = new Formula("6 / ((3.1 +2.9) - 6)");
            Assert.IsInstanceOfType(k.Evaluate(lookup), typeof(FormulaError));
        }

        /// <summary>
        /// Results to a division by zero using a variable in the lookup function, results with a formula error
        /// </summary>
        [TestMethod]
        public void divideByZero3()
        {
            Formula k = new Formula(" 4 + 3 / zz");
            Assert.IsInstanceOfType(k.Evaluate(lookup), typeof(FormulaError));
        }

        /// <summary>
        /// A division by zero if performed in proper order, results with a formula error
        /// </summary>
        [TestMethod]
        public void divideByZero4()
        {
            Formula k = new Formula("(6+2+(8/(4-4)))");
            Assert.IsInstanceOfType(k.Evaluate(lookup), typeof(FormulaError));
        }

        /// <summary>
        /// Checks that the list returned for GetVariable method is empty
        /// </summary>
        [TestMethod]
        public void noVariables()
        {
            Formula k = new Formula("6+2-(4+4)");
            HashSet<string> b = (HashSet<string>)k.GetVariables();
            Assert.IsTrue(b.Count == 0);
        }

        /// <summary>
        /// Checks GetVariable method with a formula that has repeating variables
        /// </summary>
        [TestMethod]
        public void getVariables1()
        {
            Formula k = new Formula("_ + _j / v4+ cinehvif- (cinehvif)");
            //retreave the IEnumerable with a hash set
            HashSet<string> b = (HashSet<string>)k.GetVariables();
            //create a list that should be the list of variables
            List<string> allVariables = new List<string>();
            allVariables.Add("_");
            allVariables.Add("_j");
            allVariables.Add("v4");
            allVariables.Add("cinehvif");

            //make sure that the list and the hash set match
            foreach (string s in allVariables)
            {
                Assert.IsTrue(b.Contains(s));
            }
        }

        /// <summary>
        /// Checks that GetVariables is returning the correct list without a normalizer
        /// </summary>
        [TestMethod]
        public void getVariables2()
        {
            Formula k = new Formula("x + X - y + Y + z");
            //store IEnumerable in hashset
            HashSet<string> b = (HashSet<string>)k.GetVariables();
            //make a list of variables that should be returned
            List<string> allVariables = new List<string>() { "x", "X", "y", "Y", "z" };

            //check that both lists are the same
            foreach (string s in allVariables)
            {
                Assert.IsTrue(b.Contains(s));
            }
        }

        /// <summary>
        /// Checks that GetVariables is returning the correct list with a normalizer
        /// </summary>
        [TestMethod]
        public void getVariables3()
        {
            Formula k = new Formula("x + X - y + Y + z", normalizeUpper, isValidNoDigits);
            //store IEnumerable in hash set
            HashSet<string> b = (HashSet<string>)k.GetVariables();
            //make a list of what variables should be returned
            List<string> allVariables = new List<string>() { "X", "Y", "Z" };

            //check that both lists are the same
            foreach (string s in allVariables)
            {
                Assert.IsTrue(b.Contains(s));
            }
        }

        /// <summary>
        /// Check that toString deletes all whitespace and creates a single string from the formula, including parenthesis
        /// </summary>
        [TestMethod]
        public void toString1()
        {
            Formula k = new Formula("(x - j + 2 / 6)");
            string e = k.ToString();
            //create a string of what toString should return
            string b = "(x-j+2/6)";

            //check that both strings e and b are the same
            for (int i = 0; i < b.Length - 1; i++)
            {
                Assert.IsTrue(e[i] == b[i]);
            }
        }

        /// <summary>
        /// Check that toString deletes all whitespace and creates a single string from the formula using a normalizer
        /// </summary>
        [TestMethod]
        public void toString2()
        {
            Formula k = new Formula("(xf - j_ll + 2 / 6)", normalizeUpper, isValidNoDigits);
            string e = k.ToString();
            //create a string of what is expected
            string b = "(XF-J_LL+2/6)";

            //check that both strings are the same
            for (int i = 0; i < b.Length - 1; i++)
            {
                Assert.IsTrue(e[i] == b[i]);
            }
        }

        /// <summary>
        /// Check that toString deletes all whitespace and creates a single string from the formula using the reverse normalizer
        /// </summary>
        [TestMethod]
        public void toString3()
        {
            Formula k = new Formula("(xf - j_ll + 2 / 6)", normalizeReverse, isValidNoDigits);
            string e = k.ToString();
            //create a string of what is expected
            string b = "(fx-ll_j+2/6)";

            //check that both strings are the same
            for (int i = 0; i < b.Length - 1; i++)
            {
                Assert.IsTrue(e[i] == b[i]);
            }
        }

        /// <summary>
        /// Check Equals method with an object that is not a Formula
        /// </summary>
        [TestMethod]
        public void isEqual1()
        {
            Formula k = new Formula("666");
            object b = 666;

            Assert.IsFalse(k.Equals(b));
        }

        /// <summary>
        /// Check Equals method with two of the same formulas
        /// </summary>
        [TestMethod]
        public void isEqual2()
        {
            Formula k = new Formula("666");
            Formula b = new Formula("666");

            Assert.IsTrue(k.Equals(b));
        }

        /// <summary>
        /// Check equals method with two formulas that average out to have the same decimal
        /// </summary>
        [TestMethod]
        public void isEqual3()
        {
            Formula k = new Formula("1.0000000000000000000000000000000000000003");
            Formula b = new Formula("1");

            Assert.IsTrue(k.Equals(b));
        }

        /// <summary>
        /// Check equals method with the second formula as null
        /// </summary>
        [TestMethod]
        public void isEqual4()
        {
            Formula k = new Formula("karina_is_the_best");
            Formula b = null;

            Assert.IsFalse(k.Equals(b));
        }

        /// <summary>
        /// Check equals method with two different formulas
        /// </summary>
        [TestMethod]
        public void isEqual5()
        {
            Formula k = new Formula("4 - 2.3");
            Formula b = new Formula("4 - 2.03");

            Assert.IsFalse(k.Equals(b));
        }

        /// <summary>
        /// Check Equals method with a normalizer function
        /// </summary>
        [TestMethod]
        public void isEqualWithNormalizer()
        {
            Formula k = new Formula("z_x3 - 6666", normalizeUpper, isValidLastDigit);
            Formula b = new Formula("Z_X3 - 6666");

            Assert.IsTrue(k.Equals(b));
        }

        /// <summary>
        /// Check '==' with both formulas as null
        /// </summary>
        [TestMethod]
        public void equalOperatorNull1()
        {
            Formula k = null;
            Formula b = null;
            Assert.IsTrue(k == b);
        }

        /// <summary>
        /// Check '==' with the first formula as null
        /// </summary>
        [TestMethod]
        public void equalOperatorNull2()
        {
            Formula k = null;
            Formula b = new Formula("butt");
            Assert.IsFalse(k == b);
        }

        /// <summary>
        /// Check '==' with the second formula as null
        /// </summary>
        [TestMethod]
        public void equalOperatorNull3()
        {
            Formula k = new Formula("tub");
            Formula b = null;
            Assert.IsFalse(k == b);
        }

        /// <summary>
        /// Check '==' method with a nomalizer function
        /// </summary>
        [TestMethod]
        public void equalOperator1()
        {
            Formula k = new Formula("ts", normalizeReverse, isValidNoDigits);
            Formula b = new Formula("st");
            Assert.IsTrue(k == b);
        }

        /// <summary>
        /// Check '==' with two different doubles
        /// </summary>
        [TestMethod]
        public void equalOperator2()
        {
            Formula k = new Formula("8.040000000000000000000000000007");
            Formula b = new Formula("8.04");
            Assert.IsTrue(k == b);
        }

        /// <summary>
        /// Check '!=' with two null formulas
        /// </summary>
        [TestMethod]
        public void notEqualOperatorNull1()
        {
            Formula k = null;
            Formula b = null;
            Assert.IsFalse(k != b);
        }

        /// <summary>
        /// Check '!=' with the first formula as null
        /// </summary>
        [TestMethod]
        public void notEqualOperatorNull2()
        {
            Formula k = null;
            Formula b = new Formula("butt");
            Assert.IsTrue(k != b);
        }

        /// <summary>
        /// Check '!=' method with the second formula as null
        /// </summary>
        [TestMethod]
        public void notEqualOperatorNull3()
        {
            Formula k = new Formula("tub");
            Formula b = null;
            Assert.IsTrue(k != b);
        }

        /// <summary>
        /// Check '!=' with a normalizer function on one of the formulas
        /// </summary>
        [TestMethod]
        public void notEqualOperatorNormalizer()
        {
            Formula k = new Formula("kk");
            Formula b = new Formula("kk", normalizeUpper, isValidNoDigits);
            Assert.IsTrue(k != b);
        }

        /// <summary>
        /// Check HashCode with two very simular doubles
        /// </summary>
        [TestMethod]
        public void checkHashSimularDoubles1()
        {
            Formula k = new Formula("3 + 4.10000000000000002");
            Formula e = new Formula("3 + 4.10000000000000003");

            Assert.IsTrue(k.GetHashCode() == e.GetHashCode());
        }

        /// <summary>
        /// Check Hash code with two different doubles that do not equal
        /// </summary>
        [TestMethod]
        public void checkHashSimularDoubles2()
        {
            Formula k = new Formula("4.0000000000345");
            Formula b = new Formula("4.0000000000341");

            Assert.AreNotEqual(k.GetHashCode(), b.GetHashCode());
        }

        /// <summary>
        /// Check hash code results with a normalizer on one formula
        /// </summary>
        [TestMethod]
        public void checkHashNormalizer1()
        {
            Formula k = new Formula("jk +2", normalizeUpper, isValidNoDigits);
            Formula b = new Formula("JK + 2");

            Assert.AreEqual(k.GetHashCode(), b.GetHashCode());
        }

        /// <summary>
        /// Check hash code results with a normalizer on one formula
        /// </summary>
        [TestMethod]
        public void checkHashNormalizer2()
        {
            Formula k = new Formula("jjk + 4", normalizeUpper, isValidNoDigits);
            Formula b = new Formula("jjk +4");

            Assert.AreNotEqual(k.GetHashCode(), b.GetHashCode());
        }

        /// <summary>
        /// Takes a string and converts all the letter to uppercase
        /// </summary>
        /// <param name="s"></param>
        /// The token that has been read as a variable in the evaluate method
        /// <returns></returns>
        private string normalizeUpper(string s)
        {
            return s.ToUpper();
        }

        /// <summary>
        /// Takes a string and converts it to read backwards
        /// </summary>
        /// <param name="s"></param>
        /// The token that has been read as a variable in the evaluate method
        /// <returns></returns>
        private string normalizeReverse(string s)
        {
            //start a new string
            string reverse = "";
            //starting from the last character in s, building a string to the first character in s
            for (int i = s.Length - 1; i >= 0; i--)
            {
                reverse += s[i];
            }
            return reverse;
        }

        /// <summary>
        /// Checks that the last character in the string is a number
        /// </summary>
        /// <param name="s"></param>
        /// The token that is read as a variable in the Evaluate method
        /// <returns></returns>
        private bool isValidLastDigit(string s)
        {
            if (char.IsDigit(s[s.Length - 1]))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks that a string does not contain any numbers
        /// </summary>
        /// <param name="s"></param>
        /// The token that is read as a variable in the Evaluate method
        /// <returns></returns>
        private bool isValidNoDigits(string s)
        {
            foreach (char c in s)
            {
                if (char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Creates a number that matches a specific variable, return an exception and the variable passed in is not 
        /// found in the dictionary provided
        /// </summary>
        /// <param name="s"></param>
        /// The variable to be searched for
        /// <returns></returns>
        private double lookup(string s)
        {
            if (s == "A6")
            {
                return 4;
            }
            else if (s == "a6")
            {
                return 2;
            }
            else if (s == "x9")
            {
                return 6.6;
            }
            else if (s == "_999")
            {
                return 666;
            }
            else if (s == "Px")
            {
                return 68;
            }
            else if (s == "PX")
            {
                return 6;
            }
            else if (s == "pX")
            {
                return 1.004;
            }
            else if (s == "zz")
            {
                return 0.0;
            }
            throw new Exception();
        }

    }

}
