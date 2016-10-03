//Written by Karina Biancone, implementing the provided Abstract class for Spreadsheet, October 2016
//
//Version 1.1
//
//Revision History:
//  1.1     10/2/16     1:10 PM   Implemented the new setCellContents, getValue, and commented plans for Changed
//                                  Added isValid and Normalizer to Tester class, as well as some additional tests
//
// 1.2      10/3/16     12:45 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;

/// <summary>
/// This project extends Abstract's methods to form a graph that holds a group of cell's. The name for each cell 
/// must begin with a letter or underscore and may be followed by more letters, underscores, or digits. Each cell
/// holds content information which is either a string, doule, or a Formula. The Spreadsheet keeps track of what cells
/// rely on one another to insure there is no Circular Dependents.
/// Author: Karina Biancone
/// </summary>

namespace SS
{

   

    /// <summary>
    /// Inherets Abstract Spreadsheet functions
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        //a dictionary to hold existing cells, uses the valid name as its key and Cell class as value(which holds the content)
        private Dictionary<string, Cell> cellGraph;
        //create an empty dependency graph
        private DependencyGraph cellDependents;

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get;
            protected set;           

        }

        /// <summary>
        /// Constructor for spreadsheet. Makes a fresh Dictionary and depencyGraph
        /// </summary>
        public Spreadsheet() 
        {
            cellGraph = new Dictionary<string, Cell>();
            cellDependents = new DependencyGraph();
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            //check that name is valid
            if (!checkName(name))
            {
                throw new InvalidNameException();
            }
            //check if the name is an existing cell
            if (cellGraph.ContainsKey(name))
            {
                Cell wanted = cellGraph[name];
                return wanted.getContents();
            }
            //there is no such cell
            else
            {
                throw new InvalidNameException();
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(String name)
        {
            object content;
            //try to get cell content wich will check if the name is null or inalid
            try
            {
                content = GetCellContents(name);
            }
            catch (Exception)
            {
                throw new InvalidNameException();
            }
            //check if content is a double
            if (content is double)
            {
                return content;
            }
            //check if the content is a Formula
            else if (content is Formula)
            {
                Formula formula = new Formula((string)content);
                return formula.Evaluate(lookup);
            }
            //return the content as a string
            return (string)content;
        }


        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            //return each cell name in the cell graph
            foreach (KeyValuePair<string, Cell> pairs in cellGraph)
            {
                string name = pairs.Key;
                yield return name;
            }
        }


        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            //check for null formula
            if (formula == null)
            {
                throw new ArgumentNullException();
            }
            //check for valid name
            if (!checkName(name))
            {
                throw new InvalidNameException();
            }

            //create an empty hashset to hold dependents
            HashSet<string> dependeesDependents = new HashSet<string>();

            //check if cell name exists
            if (cellGraph.ContainsKey(name))
            {
                if (!checkExistingCellNewContents(name, formula))
                {
                    throw new CircularException();
                }

            }
            else
            {
                //it is a new cell
                if (!checkNewCellContents(name, formula))
                {
                    throw new CircularException();
                }
            }
            //get final list of dependents
            dependeesDependents = new HashSet<string>(GetCellsToRecalculate(name));
            return dependeesDependents;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, string text)
        {
            //check if text is null
            if (text == null)
            {
                throw new ArgumentNullException();
            }

            //check that name is valid
            if (!checkName(name))
            {
                throw new InvalidNameException();
            }

            //create an empty hashset to hold dependents
            HashSet<string> dependeesDependents = new HashSet<string>();

            //make a cell that hold a double as its content
            Cell newCell = new Cell(text);

            //update cellGraph
            addToCellGraph(name, newCell);

            //add the dependee to the top of the list
            dependeesDependents.Add(name);
            //get all direct and indirect dependents
            dependeesDependents = directAndIndirectDependents(name, dependeesDependents);
            return dependeesDependents;


        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            //check that the name is valid
            if (!checkName(name))
            {
                throw new InvalidNameException();
            }

            //make a set to hold all dependents
            HashSet<string> dependeesDependents = new HashSet<string>();

            //make a cell that hold a double as its content
            Cell newCell = new Cell(number);

            //update cellGraph
            addToCellGraph(name, newCell);

            //add the dependee to the top of the list
            dependeesDependents.Add(name);
            //get all direct and indirect dependents
            dependeesDependents = directAndIndirectDependents(name, dependeesDependents);
            return dependeesDependents;
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            //check that the name is valid
            if (!checkName(name))
            {
                throw new InvalidNameException();
            }
            //use dependency graph to get direct dependents
            return cellDependents.GetDependents(name);
        }

        /// <summary>
        /// Creates an object that holds a string, double, or formula as its content
        /// </summary>
        private class Cell
        {
            private object content;

            /// <summary>
            /// Constructor for a string
            /// </summary>
            /// <param name="cellContent"></param>
            /// the string that will be that cell's content
            public Cell(string cellContent)
            {
                content = cellContent;
            }

            /// <summary>
            /// Constructor for a double
            /// </summary>
            /// <param name="cellContent"></param>
            /// the double that will be the cell's content
            public Cell(double cellContent)
            {
                content = cellContent;
            }

            /// <summary>
            /// Constructor for a formula
            /// </summary>
            /// <param name="cellContent"></param>
            /// the formula that will be that cell's content
            public Cell(Formula cellContent)
            {
                content = cellContent;
            }

            public object getContents()
            {
                return content;
            }

        }

        /// <summary>
        /// Retreives any cell that depends on 'name'. First it retrieves any cell that depends on 'name' and adds it to the list of
        /// dependents. It then will go into each one of the dependents' dependents and so on.
        /// </summary>
        /// <param name="name"></param>
        /// the name of the cell that is being checked for dependents
        /// <param name="allDependents"></param>
        /// the set that will hold all dependents for 'name'
        /// <returns></returns>
        private HashSet<string> directAndIndirectDependents(string name, HashSet<string> allDependents)
        {
            //loop through each dependents for name
            foreach (string direct in GetDirectDependents(name))
            {
                //add to set
                allDependents.Add(direct);
                //go into thos dependents' dependents
                directAndIndirectDependents(direct, allDependents);
            }
            return allDependents;
        }

        /// <summary>
        /// Check if cellGraph(spreadsheet) already contains a key with the name passed in. If it does, update that key
        /// if not make a new pair in the cellGraph. In other words, it creates a new cell in the spreadsheet.
        /// </summary>
        /// <param name="name"></param>
        /// the name of that cell, which has already been validated
        /// <param name="newCell"></param>
        /// holds the contents to be linked to that name passed in
        private void addToCellGraph(string name, Cell newCell)
        {
            if (!cellGraph.ContainsKey(name))
            {
                //make a new key with the cell passed as its value
                cellGraph.Add(name, newCell);
            }
            else
            {
                //update the keys value with the new cell passed
                cellGraph[name] = newCell;
            }
        }

        /// <summary>
        /// Checks that name is not null and is a valid variable. For a valid variable the string must begin with a letter or underscore
        /// and can be followed by any letter, digit, or underscore. It is also case sensitive
        /// </summary>
        /// <param name="name"></param>
        /// the string that will be checked
        /// <returns></returns>
        private bool checkName(string name)
        {
            if (name == "" || name == null)
            {
                return false;
            }
            if (!Regex.IsMatch(name, @"[a-zA-Z](?: [a-zA-Z]|\d)*$", RegexOptions.IgnorePatternWhitespace))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Tries to add new content to a cell that already exists, if it throws and error and changes the cell
        /// back to its original state. Returns true if there is no circular dependencies.
        /// </summary>
        /// <param name="name"></param>
        /// The name of the cell that already exist
        /// <param name="formula"></param>
        /// The new content
        /// <returns></returns>
        private bool checkExistingCellNewContents(string name, Formula formula)
        {
            HashSet<string> oldDependees = new HashSet<string>();
            //save the content that already exists in the existing cell
            oldDependees = new HashSet<string>(cellDependents.GetDependees(name));
            //replace old dependees with new dependees
            cellDependents.ReplaceDependees(name, formula.GetVariables());
            try
            {
                GetCellsToRecalculate(name);
                //it is a valid cell
                Cell newCell = new Cell(formula);
                //update spreadsheet
                cellGraph[name] = newCell;
                return true;
            }
            catch (Exception)
            {
                //remove just added dependents
                cellDependents.ReplaceDependees(name, oldDependees);
                return false;
            }
        }

        /// <summary>
        /// Makes sure that a new cell's content will not throw a circulation error, if it does it will return false.
        /// </summary>
        /// <param name="name"></param>
        /// The name for the new cell
        /// <param name="formula"></param>
        /// The new contents for that cell
        /// <returns></returns>
        public bool checkNewCellContents(string name, Formula formula)
        {
            //add the new dependees in the new formula
            foreach (string dependee in formula.GetVariables())
            {
                cellDependents.AddDependency(dependee, name);
            }
            //check circulation
            try
            {
                GetCellsToRecalculate(name);
                //it is a valid cell
                Cell newCell = new Cell(formula);
                //update spreadsheet
                cellGraph.Add(name, newCell);
                return true;
            }
            catch (Exception)
            {
                //remove dependents and don't make a new cell for the spreadsheet
                foreach (string dependee in formula.GetVariables())
                {
                    cellDependents.RemoveDependency(dependee, name);
                }
                return false;
            }
        }

        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename"></param>
        /// 
        /// <returns></returns>
        public override string GetSavedVersion(string filename)
        {

            throw new SpreadsheetReadWriteException("Can't find a certain file.");
        }

        public override void Save(string filename)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            //check that the string is not null
            if (content == null)
            {
                throw new ArgumentNullException();
            }

            //check that the name is valid
            if (!checkName(name))
            {
                throw new InvalidNameException();
            }

            //make an empty set to hold all dependents
            HashSet<string> dependeesDependents = new HashSet<string>();

            //check if the content is a double
            double outputDouble;
            if (Double.TryParse(content, out outputDouble))
            {
                dependeesDependents = (HashSet<string>)SetCellContents(name, outputDouble);
            }
            //check if content starts with an '='
            else if (content.First() == '=')
            {
                //take the = out of content
                //try to make the content a formula                
                Formula formula = new Formula(content, Normalize, IsValid);
                //try add the formula to the cell graph
                SetCellContents(name, formula);
            }
            else
            {
                //make a cell that hold a string as its content
                Cell newCell = new Cell(content);
                //update cellGraph
                addToCellGraph(name, newCell);
                //add the dependee to the list
                dependeesDependents.Add(name);
                //get all direct and indirect dependents
                dependeesDependents = directAndIndirectDependents(name, dependeesDependents);
            }
            return dependeesDependents;
        }
    }
}
