using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;

namespace SS
{
    class Spreadsheet : AbstractSpreadsheet
    {
        //a dictionary to hold existing cells, uses the valid name as its key and Cell class as value(which holds the content)
        private Dictionary<string, Cell> cellGraph = new Dictionary<string, Cell>();
        //create an empty dependency graph
        private DependencyGraph cellDependents = new DependencyGraph();

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        public override object GetCellContents(string name)
        {
            //check that name is valid
            if (name == null || name != "")
            {
                throw new InvalidNameException();
            }
            //check if the name is an existing cell
            if (cellGraph.ContainsKey(name))
            {
                return cellGraph[name];
            }
            //there is no such cell
            else
            {
                throw new InvalidNameException();
            }
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
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            //check for null formula
            if(formula == null)
            {
                throw new ArgumentNullException();
            }
            //check for valid name
            if (checkName(name))
            {
                throw new NotImplementedException();
            }


            //check for circular dependencee
            

            //get formulas variables, for each of those varibles get a set of all its dependents, check if
            //each set contains 'name', if not move on

            HashSet<string> dependeesDependents = new HashSet<string>();
            // get direct and indirect dependents for name
             dependeesDependents = directAndIndirectDependents(name, dependeesDependents);
            //make new cell with contents as formula
            Cell newCell = new Cell(formula);


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

            //make a new set to hold names' dependents
            HashSet<string> dependeesDependents = new HashSet<string>();

            //create a cell with the names' content, which will be a string in this case
            Cell newCell = new Cell(text);

            //update cellGraph
            addToCellGraph(name, newCell);

            //add the dependee to the top of the list
            dependeesDependents.Add(name);
            //get direct and indirect dependents for name
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
        public override ISet<string> SetCellContents(string name, double number)
        {
            //check that the name is valid
            if (checkName(name))
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

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            //check that the name is valid
            if (name == null || name != "")
            {
                throw new InvalidNameException();
            }

            return cellDependents.GetDependents(name);
        }

        private class Cell
        {
            private object content;

            public Cell(string tent)
            {
                content = tent;
            }

            public Cell(double tent)
            {
                content = tent;
            }

            public Cell(Formula tent)
            {
                content = tent;
            }
        }

        private HashSet<string> directAndIndirectDependents(string name, HashSet<string> allDependents)
        {
            foreach (string direct in GetDirectDependents(name))
            {
                allDependents.Add(direct);
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

        private bool checkName(string name)
        {
            if (name == null || Regex.IsMatch(name, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*$", RegexOptions.IgnorePatternWhitespace))
            {
                return false;
            }
            return true;
        }
    }
}
