using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace FormulaTester
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void emptyTest()
        {            
            Formula empty = new Formula("");           
                                   
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void improperFirstToken1()
        {
            Formula k = new Formula("+ 2 + 8");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void improperFirstToken2()
        {
            Formula k = new Formula(") 2 + 8");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void improperFirstToken3()
        {
            Formula k = new Formula("3x - 2 + 8");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void improperFirstToken4()
        {
            Formula k = new Formula("_! - 2 + 8");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void invalidToken1()
        {
            Formula k = new Formula("6+2-8?");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void invalidToken2()
        {
            Formula k = new Formula("6+2 + {3 - 8");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void invalidToken3()
        {
            Formula k = new Formula("=6+2-8");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void invalidToken4()
        {
            Formula k = new Formula("6 @^ + 9");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void parenthesisBalance1()
        {
            Formula k = new Formula("7 + ((8 - 2 + 6)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void parenthesisBalance2()
        {
            Formula k = new Formula("6 - 3 + (3*1) + 3)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void parenthesisBalance3()
        {
            Formula k = new Formula("(7 + (1) - 3 + 8");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeNumberError1()
        {
            Formula k = new Formula("(7 * 2) 3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeNumberError2()
        {
            Formula k = new Formula("7 * 2 3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeNumberError3()
        {
            Formula k = new Formula("(7 * 3) + x4 3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeVariableError1()
        {
            Formula k = new Formula("(7 * 2) + (6) x4");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeVariableError2()
        {
            Formula k = new Formula("(7 * 2) + 6 x4");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeVariableError3()
        {
            Formula k = new Formula("(7 * 2) + y4 x4");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeLParenthesisError1()
        {
            Formula k = new Formula("(7 * 2)(3 - 4)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeLParenthesisError2()
        {
            Formula k = new Formula("7(3 - 4)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeLParenthesisError3()
        {
            Formula k = new Formula("(3 - 4) + x3 (2 + 3)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeRParenthesisError1()
        {
            Formula k = new Formula("(3 + 2) - (+)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeRParenthesisError2()
        {
            Formula k = new Formula("3 + ()");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeOperator1()
        {
            Formula k = new Formula("( 3 + 2) - (* 2)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void beforeOperator2()
        {
            Formula k = new Formula("( 3 + 2) + x3 - - 2)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void lastToken1()
        {
            Formula k = new Formula("6 + 2 -");
        }

        [TestMethod]

        public void lastToken2()
        {
            Formula k = new Formula("6 + 2 - x");
        }

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
    }
}
