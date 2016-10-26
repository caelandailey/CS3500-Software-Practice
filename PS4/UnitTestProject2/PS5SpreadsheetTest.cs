//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using SS;
//using System.Collections.Generic;

////Author: Karina Biancone
//namespace SS
//{
//    [TestClass]
//    public class PS5SpreadsheetTest
//    {
//        /// <summary>
//        /// Creates a saved spreadsheet
//        /// </summary>
//        [TestMethod]
//        public void checkSave1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("A2", "4.0");
//            k.Save("1.1.xml");

//            Spreadsheet b = new Spreadsheet("1.1.xml", isValidLastDigit, normalizeUpper, "1.2");

//            Assert.IsTrue(new HashSet<string>(k.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>(b.GetNamesOfAllNonemptyCells())));

//        }

//        /// <summary>
//        /// Save a spreadsheet with nothing in it
//        /// </summary>
//        [TestMethod]
//        public void checkSave2()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.Save("nothing.xml");

//            Spreadsheet b = new Spreadsheet("nothing.xml", isValidLastDigit, normalizeUpper, "1.2");

//            Assert.IsTrue(new HashSet<string>(k.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>(b.GetNamesOfAllNonemptyCells())));

//        }

//        /// <summary>
//        /// Save a spreadsheet that is all formulas
//        /// </summary>
//        [TestMethod]
//        public void checkSave3()
//        {
//            Spreadsheet k = new Spreadsheet(isValidLastDigit, normalizeUpper, "default");
//            k.SetContentsOfCell("e1", "=5 - 8");
//            k.SetContentsOfCell("a1", "=e1 + 2");
//            k.SetContentsOfCell("k13", "=3 + 4");

//            k.Save("allFormulas.xml");

//            Spreadsheet b = new Spreadsheet("allFormulas.xml", isValidLastDigit, normalizeUpper, "1.2");

//            Assert.IsTrue(new HashSet<string>(k.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>(b.GetNamesOfAllNonemptyCells())));
            
//        }

//        /// <summary>
//        /// creates a cell with a double with an invalid name
//        /// </summary>
//        [TestMethod] 
//        [ExpectedException(typeof (InvalidNameException))] 
//        public void invalidName1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("1a", "2");
//        }

//        /// <summary>
//        /// Tries to add a cell with a name that has an underscore, throws invalid name
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void invalidName2()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("_a3", "poo");
//        }

//        /// <summary>
//        /// Tries to add a cell with a name that has a letter on the end, throws invalid name
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void invalidName3()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("a32a", "poo");
//        }

//        /// <summary>
//        /// Tries to add a cell with a name that has an invalid character on the end, throws invalid name
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void invalidName4()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("AA2&", "poo");
//        }

//        /// <summary>
//        /// Tries to add a cell with a name that is null, throws invalid name
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof (InvalidNameException))]
//        public void invalidName5()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell(null, "ya");
//        }

//        /// <summary>
//        /// Adds a cell with a valid name but normalizer makes it invalid, throws invalid name
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof (InvalidNameException))]
//        public void invalidName6()
//        {
//            Spreadsheet k = new Spreadsheet(isValidLastDigit, normalizeReverse, "1.2");
//            k.SetContentsOfCell("e2", "=3");
//        }

//        /// <summary>
//        /// Test a name that is all letters, thows invalid name exception
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void invalidName7()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("eeee", "EEEE");
//        }

//        /// <summary>
//        /// Sets the content as null, throws null exception
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void nullContentSet()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("g5", null);
//        }

//        /// <summary>
//        /// Makes a new spreadsheet and checks that change is false
//        /// </summary>
//        [TestMethod]
//        public void changed1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            Assert.IsFalse(k.Changed);
//        }

//        /// <summary>
//        /// Makes a new spreadsheet using a different constructor and checks that change is false
//        /// </summary>
//        [TestMethod]
//        public void changed2()
//        {
//            Spreadsheet k = new Spreadsheet(isValidLastDigit, normalizeUpper, "1.1");
//            Assert.IsFalse(k.Changed);
//        }

//        /// <summary>
//        /// Makes a new spreadsheet with an xml file and checks that change is false
//        /// </summary>
//        [TestMethod]
//        public void changed3()
//        {
//            Spreadsheet k = new Spreadsheet("default.xml", isValidLastDigit, normalizeUpper, "1.2");
//            Assert.IsFalse(k.Changed);
//        }

//        /// <summary>
//        /// Adds something to a spreadsheet, checks that changed is true
//        /// </summary>
//        [TestMethod]
//        public void changed4()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("AAA2", "4.0");
//            Assert.IsTrue(k.Changed);
//        }

//        /// <summary>
//        /// Adds something to a spreadsheet that was created with a different constructor, checks that changed is true
//        /// </summary>
//        [TestMethod]
//        public void changed5()
//        {
//            Spreadsheet k = new Spreadsheet(isValidLastDigit, normalizeUpper, "1.1");
//            k.SetContentsOfCell("QI2", "NO");
//            Assert.IsTrue(k.Changed);
//        }

//        /// <summary>
//        /// Adds something to a spreadsheet with an xml file, checks that changed is true
//        /// </summary>
//        [TestMethod]
//        public void changed6()
//        {
//            Spreadsheet k = new Spreadsheet("default.xml", isValidLastDigit, normalizeUpper, "1.2");
//            k.SetContentsOfCell("h3", "2");
//            Assert.IsTrue(k.Changed);
//        }

//        /// <summary>
//        /// Saves a spreadsheet and checks that changed goes back to false
//        /// </summary>
//        [TestMethod]
//        public void changed7()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("we9", "we");
//            k.Save("1.2.xml");
//            Assert.IsFalse(k.Changed);
//        }

//        /// <summary>
//        /// Sets a cell that doesn't have a content, therefor was not changed
//        /// </summary>
//        [TestMethod]
//        public void changed8()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("RE2", "");
//            Assert.IsFalse(k.Changed);
//        }

//        /// <summary>
//        /// Get a cells content that has not been added to the cell graph, therefor there is no content
//        /// </summary>
//        [TestMethod()]
//        public void getEmptyCell1()
//        {
//            Spreadsheet s = new Spreadsheet();
//            Assert.AreEqual("", s.GetCellContents("A2"));
//        }

//        /// <summary>
//        /// Gets a cell's value that is not in the cell graph
//        /// </summary>
//        [TestMethod()]
//        public void getEmptyCell2()
//        {
//            Spreadsheet s = new Spreadsheet();
//            Assert.AreEqual("", s.GetCellValue("A2"));
//        }

//        /// <summary>
//        /// Searches for a content with a null name, throws invalid name exception
//        /// </summary>
//        [TestMethod()]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void getNullCell1()
//        {
//            Spreadsheet s = new Spreadsheet();
//            s.GetCellContents(null);
//        }

//        /// <summary>
//        /// Get the value of a cell by calling null name, throws invalid name exception
//        /// </summary>
//        [TestMethod()]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void getNullCell2()
//        {
//            Spreadsheet s = new Spreadsheet();
//            s.GetCellValue(null);
//        }

//        /// <summary>
//        /// Get the contents of a cell usin an invalid variable, throws invalid name exception
//        /// </summary>
//        [TestMethod()]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void invalidGetCellName1()
//        {
//            Spreadsheet s = new Spreadsheet();
//            s.GetCellContents("1AA");
//        }

//        /// <summary>
//        /// Get the value of a cell with an invalid name, throws invalid name exception
//        /// </summary>
//        [TestMethod()]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void invalidGetCellName2()
//        {
//            Spreadsheet s = new Spreadsheet();
//            s.GetCellValue("1AA");
//        }

//        /// <summary>
//        /// Make sure that the read file method is properly reading and storing the information passed in
//        /// </summary>
//        [TestMethod()]
//        public void testRead1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("A2", "4");
//            k.SetContentsOfCell("RE2", "");
//            k.SetContentsOfCell("QI2", "NO");

//            k.Save("saveToRead.xml");

//            Spreadsheet b = new Spreadsheet("saveToRead.xml", isValidLastDigit, normalizeUpper, "1.2");

//            Assert.IsTrue(new HashSet<string>(k.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>(b.GetNamesOfAllNonemptyCells())));

//        }

//        /// <summary>
//        /// Makes sure that a cell is not added to spreadsheet if content is empty
//        /// </summary>
//        [TestMethod]
//        public void setEmptyContents()
//        {
//            Spreadsheet k = new Spreadsheet(isValidLastDigit, normalizeUpper, "1.3");
//            k.SetContentsOfCell("aa3", "30");
//            k.SetContentsOfCell("fd2", "");

//            Assert.IsTrue(new HashSet<string>(k.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string> { "AA3" } ));
//        }

//        /// <summary>
//        /// Adds a double that has dependents, checks that dependents were updated
//        /// </summary>
//        [TestMethod]
  
//        public void setDoubleContents()
        
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("a1", "=3+3");
//            k.SetContentsOfCell("b1", "=a1");
//            k.SetContentsOfCell("b2", "=a3");

//            Assert.IsTrue(new HashSet<string>(k.SetContentsOfCell("a3", "4")).SetEquals(new HashSet<string> {"a3", "b2" }));

//            Assert.AreEqual(4.0, k.GetCellValue("b2"));
//        }

//        ///// <summary>
//        ///// Adds a string that has dependents, checks dependents value
//        ///// </summary>
//        //[TestMethod]
//        //[ExpectedException(typeof (ArgumentException))]
//        //public void setStringContents()
//        //{
//        //    Spreadsheet k = new Spreadsheet();
//        //    k.SetContentsOfCell("a1", "=3+3");
//        //    k.SetContentsOfCell("b1", "=a1");
//        //    k.SetContentsOfCell("b2", "=a3");

//        //    Assert.AreEqual(0.0, k.GetCellValue("b2"));

//        //    Assert.IsTrue(new HashSet<string>(k.SetContentsOfCell("a3", "error")).SetEquals(new HashSet<string> { "a3", "b2" }));

//        //    k.GetCellValue("b2");
//        //}

//        /// <summary>
//        /// Adds a string that has dependents
//        /// </summary>
//        [TestMethod]

//        public void setStringContents()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("a1", "=3+3");
//            k.SetContentsOfCell("b1", "=a1");
//            k.SetContentsOfCell("a3", "2");
//            k.SetContentsOfCell("b2", "=a3");

//            Assert.AreEqual(2.0, k.GetCellValue("b2"));

//            Assert.IsTrue(new HashSet<string>(k.SetContentsOfCell("a3", "error")).SetEquals(new HashSet<string> { "a3", "b2" }));

//        }

//        /// <summary>
//        /// Adds a formula that has dependents, checks dependents value was updated
//        /// </summary>
//        [TestMethod]
//        public void setFormulaContents1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("a1", "=3+3");
//            k.SetContentsOfCell("b1", "=a1");
//            k.SetContentsOfCell("a3", "7");
//            k.SetContentsOfCell("b2", "=a3");

//            Assert.AreEqual(7.0, k.GetCellValue("b2"));
            
//            Assert.IsTrue(new HashSet<string>(k.SetContentsOfCell("a3", "=8*4 + a1")).SetEquals(new HashSet<string> { "a3", "b2" }));

//            Assert.AreEqual(38.0, k.GetCellValue("b2"));
//        }

//        /// <summary>
//        /// Changes a cell with a formula that has dependents already, checks new dependents value was updated
//        /// </summary>
//        [TestMethod]
//        public void setFormulaContents2()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("a1", "=3+3");
//            k.SetContentsOfCell("b1", "=a1");
//            k.SetContentsOfCell("b2", "=a3");
//            k.SetContentsOfCell("c3", "=b1");           

//            Assert.IsTrue(new HashSet<string>(k.SetContentsOfCell("a1", "=3+3")).SetEquals(new HashSet<string> { "a1", "b1", "c3" }));

//            k.SetContentsOfCell("b1", "=a3");
            
//            Assert.IsTrue(new HashSet<string>(k.SetContentsOfCell("a1", "3")).SetEquals(new HashSet<string> { "a1" }));
            
//        }

//        /// <summary>
//        /// Saves a file whos version is "default"
//        /// </summary>
//        [TestMethod]
//        public void getSaved1()
//        {
//            Spreadsheet k = new Spreadsheet();

//            k.Save("default.xml");

//            Assert.AreEqual("default", k.GetSavedVersion("default.xml"));
//        }

//        /// <summary>
//        /// Saves a file whos version is "1.2"
//        /// </summary>
//        [TestMethod]
//        public void getSaved2()
//        {
//            Spreadsheet k = new Spreadsheet(isValidLastDigit, normalizeReverse, "1.2");

//            k.Save("2ndConstructor.xml");

//            Assert.AreEqual("1.2", k.GetSavedVersion("2ndConstructor.xml"));
//        }

//        /// <summary>
//        /// Saves a file whos version is "default"
//        /// </summary>
//        [TestMethod]
//        public void getSaved3()
//        {
//            Spreadsheet b = new Spreadsheet();
//            b.Save("first.xml");

//            Spreadsheet k = new Spreadsheet("first.xml", isValidLastDigit, normalizeReverse, "1.2");

//            k.Save("3rdConstructor.xml");

//            Assert.AreEqual("default", k.GetSavedVersion("3rdConstructor.xml"));
//        }

//        /// <summary>
//        /// Passes in a file that does not have a version to read
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof (SpreadsheetReadWriteException))]
//        public void invalidVersion()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.GetSavedVersion("invalidFile.xml");
//        }

//        /// <summary>
//        /// Tests that a circular dependency is caught
//        /// </summary>
//        [TestMethod()]
//        [ExpectedException(typeof(CircularException))]
//        public void circularDependency()
//        {
//            Spreadsheet s = new Spreadsheet();
//            s.SetContentsOfCell("A1", "=A2+A3");
//            s.SetContentsOfCell("A3", "=A4+A5");
//            s.SetContentsOfCell("A5", "=A6+A7");
//            s.SetContentsOfCell("A7", "=A1+A1");

//            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string> { "A1", "A3", "A5" }));
//        }


//        /// <summary>
//        /// Attemps to get names of all cells from an empty spreadsheet
//        /// </summary>
//        [TestMethod()]
//        public void getNamesEmpty()
//        {
//            Spreadsheet s = new Spreadsheet();
//            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
//        }


//        /// <summary>
//        /// Adds 98 values to the spreadsheet, which rely on one another
//        /// </summary>
//        [TestMethod()]
//        public void stressTest()
//        {
//            Spreadsheet s = new Spreadsheet();
//            ISet<String> cells = new HashSet<string>();
//            for (int i = 1; i < 200; i++)
//            {
//                cells.Add("A" + i);
//                Assert.IsTrue(cells.SetEquals(s.SetContentsOfCell("A" + i, "=A" + (i + 1))));
//            }
//        }

//        /// <summary>
//        /// Tries to get a cell content with a name that is not valid after normalizer
//        /// </summary>
//        [TestMethod]
//        [ExpectedException (typeof (InvalidNameException))]
//        public void invalidNameGet1()
//        {
//            Spreadsheet k = new Spreadsheet(isValidLastDigit, normalizeReverse, "wrong");

//            k.GetCellContents("e4");
//        }

//        /// <summary>
//        /// Tries to get a cell value with a name that is not valid after normalizer
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void invalidNameGet2()
//        {
//            Spreadsheet k = new Spreadsheet(isValidLastDigit, normalizeReverse, "wrong");

//            k.GetCellValue("e4");
//        }

//        /// <summary>
//        /// A spreadsheet is created and saved, a new spreadsheet tries to read it but the normalizer
//        /// makes the names invalid
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof (SpreadsheetReadWriteException))]
//        public void readInvalidName()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("a1", "=3+3");
//            k.SetContentsOfCell("b1", "=a1");
//            k.SetContentsOfCell("b2", "=a3");
//            k.SetContentsOfCell("c3", "=b1");

//            k.Save("forRead.xml");

//            Spreadsheet b = new Spreadsheet("forRead.xml", isValidLastDigit, normalizeReverse, "1.2");
                        
//        }

//        /// <summary>
//        /// Tests that a circular dependency is caught for a cell that already contained content
//        /// </summary>
//        [TestMethod()]
//        [ExpectedException(typeof(CircularException))]
//        public void circularDependency2()
//        {
//            Spreadsheet s = new Spreadsheet();
//            s.SetContentsOfCell("A1", "=A2+A3");
//            s.SetContentsOfCell("A3", "=A4+A5");
//            s.SetContentsOfCell("A5", "=A6+A7");
//            s.SetContentsOfCell("A7", "=7");

//            s.SetContentsOfCell("A7", "=A1 +2");

//            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string> { "A1", "A3", "A5", "A7" }));

//            Assert.AreEqual(7.0, s.GetCellValue("A7"));
//        }

//        /// <summary>
//        /// Adds an empty string as the name to spreadsheet, throws invalid name exception
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof (InvalidNameException))]
//        public void emptyName1()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("", "7");

//        }

//        /// <summary>
//        /// Checks that dependents are updated after changing a cell's content
//        /// </summary>
//        [TestMethod]
//        public void checkDependents()
//        {
//            Spreadsheet k = new Spreadsheet();            
//            k.SetContentsOfCell("b1", "=a1");
//            k.SetContentsOfCell("b2", "=a3");
//            k.SetContentsOfCell("c3", "=b1");

//            Assert.IsTrue(new HashSet<string>(k.SetContentsOfCell("a1", "=3+3")).SetEquals(new HashSet<string> { "a1", "b1", "c3" }));

//            k.SetContentsOfCell("c3", "2");

//            Assert.IsTrue(new HashSet<string>(k.SetContentsOfCell("a1", "=3+3")).SetEquals(new HashSet<string> { "a1", "b1" }));

//        }

//        /// <summary>
//        /// searches for a value using a name that is a blank string, throws exception
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void emptyName2()
//        {
//            Spreadsheet k = new Spreadsheet();
//            k.SetContentsOfCell("er4", "7");
//            k.GetCellValue("");

//        }

//        /// <summary>
//        /// Checks depedents for converting a formula to double
//        /// </summary>
//        [TestMethod()]
//        public void convertingFormulaToDouble()
//        {
//            Spreadsheet s = new Spreadsheet();
//            for (int i = 0; i < 500; i++)
//            {
//                s.SetContentsOfCell("A1" + i, "=A1" + (i + 1));
//            }
//            HashSet<string> firstCells = new HashSet<string>();
//            HashSet<string> lastCells = new HashSet<string>();
//            for (int i = 0; i < 250; i++)
//            {
//                firstCells.Add("A1" + i);
//                lastCells.Add("A1" + (i + 250));
//            }
//            Assert.IsTrue(s.SetContentsOfCell("A1249", "25.0").SetEquals(firstCells));
//            Assert.IsTrue(s.SetContentsOfCell("A1499", "0").SetEquals(lastCells));
//        }

//        /// <summary>
//        /// Has multiple cell's dependend on one another for the value
//        /// </summary>
//        [TestMethod]
//        public void complicatedValues()
//        {
//            Spreadsheet k = new Spreadsheet();

            
//            k.SetContentsOfCell("A1", "1");
//            k.SetContentsOfCell("a1", "A1");
//            k.SetContentsOfCell("b1", "=A1 +2");
//            k.SetContentsOfCell("c1", "=a2 *2");
//            k.SetContentsOfCell("a2", "=A1 +b1");
//            k.SetContentsOfCell("b2", "=c1*b1");
//            k.SetContentsOfCell("a3", "=A1+c1");

//            Assert.AreEqual(3.0, k.GetCellValue("b1"));
//            Assert.AreEqual("A1", k.GetCellValue("a1"));
//            Assert.AreEqual(4.0, k.GetCellValue("a2"));
//            Assert.AreEqual(8.0, k.GetCellValue("c1"));
//            Assert.AreEqual(24.0, k.GetCellValue("b2"));
//            Assert.AreEqual(9.0, k.GetCellValue("a3"));
//        }

//        ///////////////////////////
//        /// <summary>
//        /// Takes a string and converts all the letter to uppercase
//        /// </summary>
//        /// <param name="s"></param>
//        /// The token that has been read as a variable in the evaluate method
//        /// <returns></returns>
//        private string normalizeUpper(string s)
//        {
//            return s.ToUpper();
//        }

//        /// <summary>
//        /// Takes a string and converts it to read backwards
//        /// </summary>
//        /// <param name="s"></param>
//        /// The token that has been read as a variable in the evaluate method
//        /// <returns></returns>
//        private string normalizeReverse(string s)
//        {
//            //start a new string
//            string reverse = "";
//            //starting from the last character in s, building a string to the first character in s
//            for (int i = s.Length - 1; i >= 0; i--)
//            {
//                reverse += s[i];
//            }
//            return reverse;
//        }

//        /// <summary>
//        /// Checks that the last character in the string is a number
//        /// </summary>
//        /// <param name="s"></param>
//        /// The token that is read as a variable in the Evaluate method
//        /// <returns></returns>
//        private bool isValidLastDigit(string s)
//        {
//            if (char.IsDigit(s[s.Length - 1]))
//            {
//                return true;
//            }
//            return false;
//        }


//    }

//}
