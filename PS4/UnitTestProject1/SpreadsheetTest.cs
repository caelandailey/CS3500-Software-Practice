//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using SpreadsheetUtilities;
//using SS;
//using System.Collections.Generic;
///// <summary>
///// Checks all methods in Spreadsheet project. Throws errors for invalid names for cells, invalid content, and
///// circular dependencies on multiple cells.
///// Author: Karina Biancone
///// </summary>
//namespace SpreadsheetTest
//{
//    [TestClass]
//    public class SpreadsheetTest
//    {
//        /// <summary>
//        /// Names a cell with a double using null, throws invalid name exception
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void nullNameSet1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents(null, 3.0);
//        }

//        /// <summary>
//        /// Names a cell with a string using null, throws invalid name exception
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void nullNameSet2()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents(null, "null");
//        }

//        /// <summary>
//        /// Names a cell with an double as its content, using an invalid string,throws invalid name exception
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void invalidNameSet1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("666", 3.0);
//        }

//        /// <summary>
//        /// Names a cell with an string as its content, using an invalid string, throws invalid name exception
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void invalidNameSet2()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("____3@", "____3@");
//        }


//        /// <summary>
//        /// Make an empty spreadsheet and that there are no names in it
//        /// </summary>
//        [TestMethod]
//        public void emptySpreadsheetGetNames1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            HashSet<string> b = new HashSet<string>();
//            Assert.IsTrue(b.SetEquals(k.GetNamesOfAllNonemptyCells()));
//        }

//        /// <summary>
//        /// Adds multiple cells with simular names(case sensitive) and checks the list of names returned
//        /// </summary>
//        [TestMethod]
//        public void correctNames1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("A1", 2);
//            k.SetCellContents("a1", "YA");
//            k.SetCellContents("_1", "=6+2");

//            //expected list to be returned
//            HashSet<string> b = new HashSet<string> { "A1", "a1", "_1" };
//            Assert.IsTrue(b.SetEquals(k.GetNamesOfAllNonemptyCells()));
//        }

//        /// <summary>
//        /// Add multiple cells, add a cell with a name that already exists, check the list of names that the spreadsheet contains
//        /// </summary>
//        [TestMethod]
//        public void correctNames2()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("A1", 2);
//            k.SetCellContents("a1", "YA");
//            k.SetCellContents("_1", "=6+2");
//            //replace A1 contents with a formula
//            k.SetCellContents("A1", "=1");

//            //expected list to be returned
//            HashSet<string> b = new HashSet<string> { "A1", "a1", "_1" };
//            Assert.IsTrue(b.SetEquals(k.GetNamesOfAllNonemptyCells()));
//        }

//        /// <summary>
//        /// Calls to get contents on a cell with a null name,throws invalid name exception
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void nullNameGetContents()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("C1", 3);
//            k.GetCellContents(null);
//        }

//        /// <summary>
//        /// Calls to get contents with an invalid name, throws invalid name exception
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void invalidNameGetContents1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("yessss", 2);
//            k.GetCellContents("1yesss");
//        }

//        /// <summary>
//        /// Calls to get contents with a valid name that is not in the spreadsheet, throws invalid name exception
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void invalidNameGetContents2()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("yessss", "yesss");
//            k.GetCellContents("yesss");
//        }

//        /// <summary>
//        /// Add multiple cells with different contents, check that a string is returned
//        /// </summary>
//        [TestMethod]
//        public void correctContent1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("ok", "NOT");
//            k.SetCellContents("mk", "nOT");
//            k.SetCellContents("okk", 5.44);
//            k.SetCellContents("ook", "=not");

//            string b = "NOT";

//            Assert.AreEqual(b, k.GetCellContents("ok"));
//        }

//        /// <summary>
//        /// Add multiple cells with different contents, check that a double is returned
//        /// </summary>
//        [TestMethod]
//        public void correctContent2()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("ok", "NOT");
//            k.SetCellContents("okk", 5.44);
//            k.SetCellContents("ook", "=not");

//            double b = 5.44;

//            Assert.AreEqual(b, k.GetCellContents("okk"));
//        }

//        /// <summary>
//        /// Add multiple cells with different contents, check that correct formula is returned
//        /// </summary>
//        [TestMethod]
//        public void correctContent3()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("ok", "NOT");
//            k.SetCellContents("okk", "5.44");
//            k.SetCellContents("ook", "=not");
//            k.SetCellContents("OOk", "=ces");

//            Formula b = new Formula("not");

//            Assert.IsTrue(b.Equals(k.GetCellContents("ook")));
//        }

//        /// <summary>
//        /// Add a cell that has a contents as a float, be sure that it converts to a double correctly
//        /// </summary>
//        [TestMethod]
//        public void setDouble1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("poop", .0000000003);          
//            double zero;
//            Double.TryParse(".0000000003", out zero);

//            Assert.AreEqual(zero, k.GetCellContents("poop"));
//        }

//        /// <summary>
//        /// Replace a cell's constant with a double
//        /// </summary>
//        [TestMethod]
//        public void setDouble2()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("A1", "##$$$#");
//            k.SetCellContents("A1", 420);
//            Assert.AreEqual(420.0, k.GetCellContents("A1"));
//        }

//        /// <summary>
//        /// Add a cell who's content is a double that has no dependents
//        /// </summary>
//        [TestMethod]
//        public void doubleDependents()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("aa", "id");
//            HashSet<string> depends = (HashSet<string>)k.SetCellContents("AA", 39);

//            Assert.IsTrue(depends.Contains("AA") && depends.Count == 1);
            
//        }

//        /// <summary>
//        /// Adds a cell with the contents as a null string, thros argument null exception
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void stringNull()
//        {
//            Spreadsheet k = new Spreadsheet();
//            string b = null;
//            k.SetCellContents("AA", b);
//        }

//        /// <summary>
//        /// Sets an empty string as a cell's content
//        /// </summary>
//        [TestMethod]
//        public void setString1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("AA", "");

//            Assert.AreEqual("", k.GetCellContents("AA"));
//        }

//        /// <summary>
//        /// Updates a cell that already exists to a string
//        /// </summary>
//        [TestMethod]
//        public void setString2()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("AF", 78);
//            k.SetCellContents("AF", "JF");

//            Assert.AreEqual("JF", k.GetCellContents("AF"));
//        }

//        /// <summary>
//        /// Add cells that don't depend on one another
//        /// </summary>
//        [TestMethod]
//        public void stringDependents()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("aa", 78);
//            HashSet<string> depends = (HashSet<string>)k.SetCellContents("AA", "poop");

//            Assert.IsTrue(depends.Contains("AA") && depends.Count == 1);
//        }

//        /// <summary>
//        /// Add a cell who's content is a Formula that is equal to null, throws argument null exception
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void nullFormula()
//        {
//            Spreadsheet k = new Spreadsheet();
//            Formula b = null;
//            k.SetCellContents("AA", b);
//        }

//        /// <summary>
//        /// Add a cell with the contents as a formula
//        /// </summary>
//        [TestMethod]
//        public void setFormula1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("_", "=6+6+6");
//            Formula b = new Formula("6+6+6");

//            Assert.IsTrue(b.Equals(k.GetCellContents("_")));
//        }

//        /// <summary>
//        /// Replace a cell's content with a formula
//        /// </summary>
//        [TestMethod]
//        public void setFormula2()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("RE2", "RE2");
//            k.SetCellContents("RE2", "=3");
//            Formula b = new Formula("3");

//            Assert.IsTrue(b.Equals(k.GetCellContents("RE2")));
//        }

//        /// <summary>
//        /// check the dependents for a cell with content as a double
//        /// </summary>
//        [TestMethod]
//        public void doubleDependentsCircular1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("A2", "=B1");
//            k.SetCellContents("B1", "=D2 * 3");
//            k.SetCellContents("D2", "=C2");

//            Assert.IsTrue(new HashSet<string> { "C2", "D2", "B1", "A2" }.SetEquals(k.SetCellContents("C2", 3)));
//        }

//        /// <summary>
//        /// check dependents for a cell with a string as its contents
//        /// </summary>
//        [TestMethod]
//        public void stringDependentsCircular1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("A2", "=B1");
//            k.SetCellContents("B1", "=D2 * 3");
//            k.SetCellContents("D2", "=C2");

//            Assert.IsTrue(new HashSet<string> { "C2", "D2", "B1", "A2" }.SetEquals(k.SetCellContents("C2", "3")));
//        }

//        /// <summary>
//        /// Check the dependents on a cell with a formula 
//        /// </summary>
//        [TestMethod]
//        public void formulaDependentsCircular1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("A2", "=B1");
//            k.SetCellContents("B1", "=D2 * 3");
//            k.SetCellContents("D2", "=C2");

//            Assert.IsTrue(new HashSet<string> { "C2", "D2", "B1", "A2" }.SetEquals(k.SetCellContents("C2", "=3+3")));
//        }

//        /// <summary>
//        /// Has two cell names in one cell, check one of the cell dependents
//        /// </summary>
//        [TestMethod]
//        public void formulaDependentsCircular2()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("A2", "=B1");
//            k.SetCellContents("B1", "=D2 * 3");
//            k.SetCellContents("D2", "=C2");
//            k.SetCellContents("D3", "=A2");
//            k.SetCellContents("C2", "=E1 + A1");

//            Assert.IsTrue(new HashSet<string> { "A1","C2", "D2", "B1", "A2", "D3" }.SetEquals(k.SetCellContents("A1", "9")));
//        }

//        /// <summary>
//        /// Replaces a cell that has a string as a content to a formula as its content
//        /// </summary>
//        [TestMethod]
//        public void replaceFormula()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("A2", "=B1");
//            k.SetCellContents("B1", "=D2 * 3");
//            k.SetCellContents("D2", "=C2");
//            k.SetCellContents("D3", "=A2");
//            k.SetCellContents("D3", "=A2");
//            k.SetCellContents("C2", 3);

//            Assert.IsTrue(new HashSet<string> { "C2", "D2", "B1", "A2", "D3" }.SetEquals(k.SetCellContents("C2", "=E1 + A1")));
//        }

//        /// <summary>
//        /// Finds a circular dependents
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(CircularException))]
//        public void dependentsCircularFail1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("a1", "=b1 * 2");
//            k.SetCellContents("b1", "=c1 * 2");
//            k.SetCellContents("c1", "=a1 * 2");
//        }

//        /// <summary>
//        /// Change a cell's content from a double to a formula, makes it a circular dependents
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(CircularException))]
//        public void dependentsCircularFail2()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("a1", "=b1 * 2");
//            k.SetCellContents("b1", "=c1 * 2");
//            k.SetCellContents("c1", 4.20);
//            k.SetCellContents("c1", "=a1 * 2");
//        }

//        /// <summary>
//        /// Change a cell's content from a double to a formula, makes it a circular dependents
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(CircularException))]
//        public void dependentsCircularFail3()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetCellContents("a1", "=b1 * 2");
//            k.SetCellContents("b1", "=c1 * 2");
//            k.SetCellContents("c1", "poo");
//            k.SetCellContents("c1", "=a1 * 2");
//        }


//    }


//}

    

