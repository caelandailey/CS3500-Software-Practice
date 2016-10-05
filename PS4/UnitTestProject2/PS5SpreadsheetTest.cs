using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System.Collections.Generic;

namespace SS
{
    [TestClass]
    public class PS5SpreadsheetTest
    {
        /// <summary>
        /// Creates a saved spreadsheet
        /// </summary>
        [TestMethod]
        public void checkSave()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetContentsOfCell("A2", "4.0");
            k.Save("1.1.xml");
        }

        /// <summary>
        /// creates a cell with a double with an invalid name
        /// </summary>
        [TestMethod] 
        [ExpectedException(typeof (InvalidNameException))] 
        public void invalidName1()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetContentsOfCell("1a", "2");
        }

        /// <summary>
        /// Tries to add a cell with a name that has an underscore, throws invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidName2()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetContentsOfCell("_a3", "poo");
        }

        /// <summary>
        /// Tries to add a cell with a name that has a letter on the end, throws invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidName3()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetContentsOfCell("a32a", "poo");
        }

        /// <summary>
        /// Tries to add a cell with a name that has an invalid character on the end, throws invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidName4()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetContentsOfCell("AA2&", "poo");
        }

        /// <summary>
        /// Tries to add a cell with a name that is null, throws invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void invalidName5()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetContentsOfCell(null, "ya");
        }

        /// <summary>
        /// Makes a new spreadsheet and checks that change is false
        /// </summary>
        [TestMethod]
        public void changed1()
        {
            Spreadsheet k = new Spreadsheet();
            Assert.IsFalse(k.Changed);
        }

        /// <summary>
        /// Makes a new spreadsheet using a different constructor and checks that change is false
        /// </summary>
        [TestMethod]
        public void changed2()
        {
            Spreadsheet k = new Spreadsheet(isValidLastDigit, normalizeUpper, "1.1");
            Assert.IsFalse(k.Changed);
        }

        /// <summary>
        /// Makes a new spreadsheet with an xml file and checks that change is false
        /// </summary>
        [TestMethod]
        public void changed3()
        {
            Spreadsheet k = new Spreadsheet("default.xml", isValidLastDigit, normalizeUpper, "1.2");
            Assert.IsFalse(k.Changed);
        }

        /// <summary>
        /// Adds something to a spreadsheet, checks that changed is true
        /// </summary>
        [TestMethod]
        public void changed4()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetContentsOfCell("AAA2", "4.0");
            Assert.IsTrue(k.Changed);
        }

        /// <summary>
        /// Adds something to a spreadsheet that was created with a different constructor, checks that changed is true
        /// </summary>
        [TestMethod]
        public void changed5()
        {
            Spreadsheet k = new Spreadsheet(isValidLastDigit, normalizeUpper, "1.1");
            k.SetContentsOfCell("QI2", "NO");
            Assert.IsTrue(k.Changed);
        }

        /// <summary>
        /// Adds something to a spreadsheet with an xml file, checks that changed is true
        /// </summary>
        [TestMethod]
        public void changed6()
        {
            Spreadsheet k = new Spreadsheet("default.xml", isValidLastDigit, normalizeUpper, "1.2");
            k.SetContentsOfCell("h3", "2");
            Assert.IsTrue(k.Changed);
        }

        /// <summary>
        /// Saves a spreadsheet and checks that changed goes back to false
        /// </summary>
        [TestMethod]
        public void changed7()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetContentsOfCell("we9", "we");
            k.Save("1.2.xml");
            Assert.IsFalse(k.Changed);
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
    }

}
