using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;

namespace SS
{
    class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, object> cellGraph = new Dictionary<string, object>();

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        public override object GetCellContents(string name)
        {
            if(name == null || name != "")
            {
                throw new InvalidNameException();
            }

            if (cellGraph.ContainsKey(name))
            {
                return cellGraph[name];
            }
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
            foreach (KeyValuePair<string, object> pairs in cellGraph)
            {
                string name = pairs.Key;
                yield return name;
            }
        }



        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            throw new NotImplementedException();
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
            if(text == null)
            {
                throw new ArgumentNullException();
            }
            if (name == null || name != "")
            {
                throw new InvalidNameException();
            }

            HashSet<string> dependeesDependents = new HashSet<string>();
            Cell newCell = new Cell(text);
            if (!cellGraph.ContainsKey(name))
            {
                cellGraph.Add(name, text);
            }
            else
            {
                //the cell already contains some content
                GetCellsToRecalculate(name);
            }

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
            if (name == null || name != "")
            {
                throw new InvalidNameException();
            }

            HashSet<string> dependeesDependents = new HashSet<string>();
            Cell newCell = new Cell(number);
            if (!cellGraph.ContainsKey(name))
            {   
                cellGraph.Add(name, number);
            }
            else
            {
                GetCellsToRecalculate(name);
            }

            dependeesDependents = directAndIndirectDependents(name, dependeesDependents);
            return dependeesDependents;
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            throw new NotImplementedException();
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
            foreach(string direct in GetDirectDependents(name))
            {
                allDependents.Add(direct);
                directAndIndirectDependents(direct, allDependents);
            }
            return allDependents;
        }
    }
}
