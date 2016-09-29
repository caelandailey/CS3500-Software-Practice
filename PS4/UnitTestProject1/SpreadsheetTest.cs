using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System.Collections.Generic;

namespace SpreadsheetTest
{
    [TestClass]
    public class SpreadsheetTest
    {
        /// <summary>
        /// Names a cell with a double using null, throws invalid name exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void nullNameSet1()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents(null, 3.0);
        }

        /// <summary>
        /// Names a cell with a string using null, throws invalid name exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void nullNameSet2()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents(null, "null");
        }

        /// <summary>
        /// Names a cell with a formula using null, throws invalid name exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void nullNameSet3()
        {
            Spreadsheet k = new Spreadsheet();
            Formula b = new Formula("x2");
            k.SetCellContents(null, b);
        }

        /// <summary>
        /// Names a cell with an double as its content, using an invalid string,throws invalid name exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidNameSet1()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("666", 3.0);
        }

        /// <summary>
        /// Names a cell with an string as its content, using an invalid string, throws invalid name exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidNameSet2()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("____3@", "____3@");
        }

        /// <summary>
        /// Names a cell with an formula as its content, using an invalid string, throws invalid name exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidNameSet3()
        {
            Spreadsheet k = new Spreadsheet();
            Formula b = new Formula("3 + x4");
            k.SetCellContents("", b);
        }


        /// <summary>
        /// Make an empty spreadsheet and that there are no names in it
        /// </summary>
        [TestMethod]
        public void emptySpreadsheetGetNames1()
        {
            Spreadsheet k = new Spreadsheet();
            List<string> b = new List<string>();
            Assert.AreEqual(b, k.GetNamesOfAllNonemptyCells());
        }

        /// <summary>
        /// Add cells to the spreadsheet, delete them, check if there are existing cells
        /// </summary>
        [TestMethod]
        public void emptySpreadsheetGetNames2()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("A1", 2);
            k.SetCellContents("a1", "YA");
            k.SetCellContents("_1", new Formula("6+2"));

            //remove those items
            //check that get names is empty

        }

        /// <summary>
        /// Adds multiple cells with simular names(case sensitive) and checks the list of names returned
        /// </summary>
        [TestMethod]
        public void correctNames1()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("A1", 2);
            k.SetCellContents("a1", "YA");
            k.SetCellContents("_1", new Formula("6+2"));

            //expected list to be returned
            List<string> b = new List<string> { "A1", "a1", "_1" };
            Assert.AreEqual(b, k.GetNamesOfAllNonemptyCells());
        }

        /// <summary>
        /// Add multiple cells, add a cell with a name that already exists, check the list of names that the spreadsheet contains
        /// </summary>
        [TestMethod]
        public void correctNames2()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("A1", 2);
            k.SetCellContents("a1", "YA");
            k.SetCellContents("_1", new Formula("6+2"));
            //replace A1 contents with a formula
            k.SetCellContents("A1", new Formula("1"));

            //expected list to be returned
            List<string> b = new List<string> { "A1", "a1", "_1" };
            Assert.AreEqual(b, k.GetNamesOfAllNonemptyCells());
        }

        /// <summary>
        /// Calls to get contents on a cell with a null name,throws invalid name exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void nullNameGetContents()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("C1", 3);
            k.GetCellContents(null);
        }

        /// <summary>
        /// Calls to get contents with an invalid name, throws invalid name exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidNameGetContents1()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("yessss", 2);
            k.GetCellContents("1yesss");
        }

        /// <summary>
        /// Calls to get contents with a valid name that is not in the spreadsheet, throws invalid name exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidNameGetContents2()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("yessss", "yesss");
            k.GetCellContents("yesss");
        }

        /// <summary>
        /// Add multiple cells with different contents, check that a string is returned
        /// </summary>
        [TestMethod]
        public void correctContent1()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("ok", "NOT");
            k.SetCellContents("mk", "nOT");
            k.SetCellContents("okk", 5.44);
            k.SetCellContents("ook", new Formula("not"));

            string b = "NOT";

            Assert.AreEqual(b, k.GetCellContents("ok"));
        }

        /// <summary>
        /// Add multiple cells with different contents, check that a double is returned
        /// </summary>
        [TestMethod]
        public void correctContent2()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("ok", "NOT");
            k.SetCellContents("okk", 5.44);
            k.SetCellContents("ook", new Formula("not"));

            double b = 5.44;

            Assert.AreEqual(b, k.GetCellContents("okk"));
        }

        /// <summary>
        /// Add multiple cells with different contents, check that correct formula is returned
        /// </summary>
        [TestMethod]
        public void correctContent3()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("ok", "NOT");
            k.SetCellContents("okk", 5.44);
            k.SetCellContents("ook", new Formula("not"));
            k.SetCellContents("OOk", new Formula("ces"));

            Formula b = new Formula("not");

            Assert.IsTrue(b.Equals(k.GetCellContents("ook")));
        }

        /// <summary>
        /// Add a cell that has a contents as a float, be sure that it converts to a double correctly
        /// </summary>
        [TestMethod]
        public void setDouble1()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("poop", .0000000003);          
            double zero;
            Double.TryParse(".0000000003", out zero);

            Assert.AreEqual(zero, k.GetCellContents("poop"));
        }

        /// <summary>
        /// Replace a cell's constant with a double
        /// </summary>
        [TestMethod]
        public void setDouble2()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("A1", "##$$$#");
            k.SetCellContents("A1", 420);
            Assert.AreEqual(420.0, k.GetCellContents("A1"));
        }

        /// <summary>
        /// Add a cell who's content is a double that has no dependents
        /// </summary>
        [TestMethod]
        public void doubleDependents()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("aa", "id");
            HashSet<string> depends = (HashSet<string>)k.SetCellContents("AA", 39);

            Assert.IsTrue(depends.Contains("AA") && depends.Count == 1);
            
        }

        /// <summary>
        /// Adds a cell with the contents as a null string, thros argument null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void stringNull()
        {
            Spreadsheet k = new Spreadsheet();
            string b = null;
            k.SetCellContents("AA", b);
        }

        /// <summary>
        /// Sets an empty string as a cell's content
        /// </summary>
        [TestMethod]
        public void setString1()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("AA", "");

            Assert.AreEqual("", k.GetCellContents("AA"));
        }

        /// <summary>
        /// Updates a cell that already exists to a string
        /// </summary>
        [TestMethod]
        public void setString2()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("AF", 78);
            k.SetCellContents("AF", "JF");

            Assert.AreEqual("JF", k.GetCellContents("AF"));
        }

        /// <summary>
        /// Add cells that don't depend on one another
        /// </summary>
        [TestMethod]
        public void stringDependents()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("aa", 78);
            HashSet<string> depends = (HashSet<string>)k.SetCellContents("AA", "poop");

            Assert.IsTrue(depends.Contains("AA") && depends.Count == 1);
        }

        /// <summary>
        /// Add a cell who's content is a Formula that is equal to null, throws argument null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullFormula()
        {
            Spreadsheet k = new Spreadsheet();
            Formula b = null;
            k.SetCellContents("AA", b);
        }

        /// <summary>
        /// Add a cell with the contents as a formula
        /// </summary>
        [TestMethod]
        public void setFormula1()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("_", new Formula("6+6+6"));
            Formula b = new Formula("6+6+6");

            Assert.IsTrue(b.Equals(k.GetCellContents("_")));
        }

        /// <summary>
        /// Replace a cell's content with a formula
        /// </summary>
        [TestMethod]
        public void setFormula2()
        {
            Spreadsheet k = new Spreadsheet();
            k.SetCellContents("RE2", "RE2");
            k.SetCellContents("RE2", new Formula("3"));
            Formula b = new Formula("3");

            Assert.IsTrue(b.Equals(k.GetCellContents("RE2")));
        }



    }
}
