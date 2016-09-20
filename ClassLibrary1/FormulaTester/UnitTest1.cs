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
            Formula k = new Formula("x - 2 + 8");
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
    }
}
