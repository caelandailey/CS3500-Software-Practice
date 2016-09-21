using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

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
            Formula k = new Formula("3x - 2 + 8");
        }

        /// <summary>
        /// '=6' will be read as a single token which is invalid, throws formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void improperFirstToken4()
        {
            Formula k = new Formula("=6+2-8");
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
        /// Valid code.
        /// </summary>
        [TestMethod]
        public void lastToken2()
        {
            Formula k = new Formula("6 + 2 - x");
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

        [TestMethod]

        public void lastToken4()
        {
            Formula k = new Formula("6 + 2 - 6");
        }

        //catch error after variable is normalized and isn't a variable anymore
        //catch error after checking isValid
    }
}
