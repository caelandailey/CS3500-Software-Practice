//Written by Karina Biancone, implementing the provided Abstract class for Spreadsheet, October 2016
//
//Version 1.9
//
//Revision History:
//  1.1     10/2/16     1:10 PM   Implemented the new setCellContents, getValue, and commented plans for Changed
//                                  Added isValid and Normalizer to Tester class, as well as some additional tests
//
//  1.2     10/3/16     12:45 PM    Added changes from PS4 and built 2/3 constructors
//
//  1.3                 7:20 PM     Added is valid and normalize
//
//  1.4                 11:00 PM    Started saved and get version functions
//
//  1.5     10/4/16     4:00 PM     Worked on read helper method
//
//                      8:00 PM     Created a way to start updating value and tested save method
//
//  1.6     10/5/16     8:00 AM     Made helper functions for updating the value
//
//  1.7                 3:00 PM     Finished helper functions, wrote tests for change, and invalid names
//
//  1.8                 8:00 PM     Tested all methods until I got over 95% code coverage, tweaking small lines of code
//
//  1.9     10/6/16     9:00 PM     Added more comments, a couple more formula tests, and updated readme file
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;
using System.Xml;

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
        /// Checks that a variable name for formula begins with one or more letter and ends with one or more numbers,
        /// also uses the validation passed in from constructor
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        private bool spreadsheetValid(string variable)
        {
            if (Regex.IsMatch(variable, @"^[a-zA-Z]+\d+$", RegexOptions.IgnorePatternWhitespace) && IsValid(variable))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Constructor for spreadsheet, sets the parameters, and sets cellGraph and cellDependents to null
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cellGraph = new Dictionary<string, Cell>();
            cellDependents = new DependencyGraph();
        }


        /// <summary>
        /// Constructor for spreadsheet. Makes a fresh Dictionary and depencyGraph
        /// </summary>
        public Spreadsheet() : this(s => true, n => n, "default") { }
        

        /// <summary>
        /// Creates a spreadsheet with the information from the file passed in.
        /// </summary>
        /// <param name="pathfile"></param>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(String pathfile, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cellGraph = new Dictionary<string, Cell>();
            cellDependents = new DependencyGraph();
            //read the file
            readFile(pathfile);
            
        }

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
        /// Checks if the Spreadsheet contains the variable passed in from the Formula Evaluator and gets
        /// its value. If the value is a string, a FormulaError, or is not in the cell graph it will throw an error.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        private double Lookup(string variable)
        {
            //checks if the variable is in spreadsheet
            if (cellGraph.ContainsKey(variable))
            {
                //checks if the value is a string or formula error
                if ((cellGraph[variable].getValue() is string) || (cellGraph[variable].getValue() is FormulaError))
                {
                    throw new ArgumentException();
                }
                //returns the value of the variable
                return (double)GetCellValue(variable);
            }
            else
            {
                //the variable is not in the spreadsheet, therefor its value is zero
                throw new ArgumentException();
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
            string newVersion = "";
            try
            {
                //create a file to read
                using (XmlReader read = XmlReader.Create(filename))
                {
                    //while there is still something to read
                    while (read.Read())
                    {
                        //the beginning of the spreadsheet
                        if (read.IsStartElement())
                        {
                            //find spreadsheets name
                            switch (read.Name)
                            {
                                //read the spreadsheet element
                                case "spreadsheet":
                                    //read the attribute
                                    newVersion = read["version"];
                                    break;
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception)
            {
                //the file could not open
                throw new SpreadsheetReadWriteException("Can not retrieve the version from the file.");
            }

            return newVersion;
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// </summary>
        /// <param name="filename"></param>
        public override void Save(string filename)
        {
            try
            {
                //set up xml settings
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = (" ");

                //create the xml file
                using (XmlWriter write = XmlWriter.Create(filename, settings))
                {
                    write.WriteStartDocument();
                    //Spreadsheet element
                    write.WriteStartElement("spreadsheet");
                    write.WriteAttributeString("version", Version);


                    //loop through current dependency graph and write it to the file
                    foreach (KeyValuePair<string, Cell> pair in cellGraph)
                    {
                        //make a cell
                        write.WriteStartElement("cell");

                        //name of the cell
                        write.WriteStartElement("name");
                        write.WriteString(pair.Key);
                        write.WriteEndElement();
                        //get the content of that cell
                        object content = GetCellContents(pair.Key);
                        //add a double  content element                     
                        if (content is double)
                        {                           
                            write.WriteStartElement("contents");
                            write.WriteString(content.ToString());
                            write.WriteEndElement();
                        }
                        //add a formula content element
                        else if(content is Formula)
                        {                            
                            write.WriteStartElement("contents");                                                                                
                            write.WriteString("=" + content.ToString());
                            write.WriteEndElement();
                        }
                        else
                        {
                            //content is a string
                            write.WriteStartElement("contents");
                            write.WriteString((string)content);
                            write.WriteEndElement();
                        }
                        //end cell element
                        write.WriteEndElement();
                    }
                    //end spreadsheet element
                    write.WriteEndElement();

                    write.WriteEndDocument();
                    Changed = false;
                }
            }
            //something went wrong with saving the file
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Can not save file.");
            }

        }

        /// <summary>
        /// Helper function that reads the xml file passed in. As the file is read spreadsheet is updated with
        /// the contents it contains.
        /// </summary>
        /// <param name="filename"></param>
        private void readFile(string filename)
        {
            try
            {
                using (XmlReader read = XmlReader.Create(filename))
                {
                    while (read.Read())
                    {
                        if (read.IsStartElement())
                        {
                            switch (read.Name)
                            {
                                //read the spreadsheet element
                                case "spreadsheet":
                                    //read the attribute
                                    if (!Version.Equals(read["version"]))
                                    {
                                        throw new Exception();
                                    }
                                                                        
                                    break;
                                //read a cell
                                case "cell":
                                    read.ReadToFollowing("name");
                                    //hold the name
                                    read.Read();
                                    string name = read.Value;
                                    //hold contents
                                    read.ReadToFollowing("contents");
                                    read.Read();
                                    string content = read.Value;
                                    
                                    //add this information to spreadsheet
                                    SetContentsOfCell(name, content);
                                    break;
                            }                       
                        }
                    }
                }
                Changed = false;
            }
            //something went wrong with reading the file
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Can not read the .xml file given");
            }
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

            //check name is not null
            if (name == null)
            {
                throw new InvalidNameException();
            }

            //normalize the name
            name = Normalize(name);

            //check that the name is valid
            if (!checkName(name))
            {
                throw new InvalidNameException();
            }

            //if the cell already has contents that is a formula, remove dependees
            ifCellExists(name);

            //make an empty set to hold all dependents
            HashSet<string> dependeesDependents = new HashSet<string>();

            //if the string is empty
            if (content == "")
            {
                //does have name of cell?
                return dependeesDependents;
            }
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
                content = content.TrimStart('=');
                //try to make the content a formula                
                Formula formula = new Formula(content, Normalize, spreadsheetValid);
                //try add the formula to the cell graph
                dependeesDependents = (HashSet<string>)SetCellContents(name, formula);
            }
            else
            {
                //make a cell that hold a string as its content
                dependeesDependents = (HashSet<string>)SetCellContents(name, (string)content);

            }
            //call cellGraph name's cell's evaluate foreach dependeesDependents
            foreach (string cellName in dependeesDependents)
            {
                cellGraph[cellName].updateValue(Lookup);
            }

            Changed = true;
            return dependeesDependents;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            //check if name is null
            if (name == null)
            {
                throw new InvalidNameException();
            }

            //normalize
            name = Normalize(name);

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
                return "";
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
            //check for null
            if (name == null)
            {
                throw new InvalidNameException();
            }

            ////update name to the normalized name
            name = Normalize(name);
            //check if name is valid
            if (!checkName(name))
            {
                throw new InvalidNameException();
            }
            //check for name in the cell graph
            if (cellGraph.ContainsKey(name))
            {
                return cellGraph[name].getValue();
            }
            //name was not in spreadsheet
            else
            {
                return "";
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
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {

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
            //make a set to hold all dependents
            HashSet<string> dependeesDependents = new HashSet<string>();

            //make a cell that hold a double as its content
            Cell newCell = new Cell(number);

            //update cellGraph
            addToCellGraph(name, newCell);

            //get all direct and indirect dependents
            dependeesDependents = new HashSet<string>(GetCellsToRecalculate(name));
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
            //check if name is null
            if(name == null)
            {
                throw new ArgumentNullException();
            }
            //normalize the name
            name = Normalize(name);
            //check that the name is valid
            if (!checkName(name))
            {
                throw new InvalidNameException();
            }
            //use dependency graph to get direct dependents
            return cellDependents.GetDependents(name);
        }

        /// <summary>
        /// Creates an object that holds a string, double, or formula as its content and calculates its value
        /// </summary>
        private class Cell
        {
            private object content;
            private object value;

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

            /// <summary>
            /// Returns the Cell's contents
            /// </summary>
            /// <returns></returns>
            public object getContents()
            {
                return content;
            }

            /// <summary>
            /// Returns the Cell's value
            /// </summary>
            /// <returns></returns>
            public object getValue()
            {
                return value;
            }

            /// <summary>
            /// Calculates the Cell's value
            /// </summary>
            /// <param name="Lookup"></param>
            public void updateValue(Func<string, double> Lookup)
            {
                //if content double, value
                if (content is double)
                {
                    value = content;
                }
                //if string, value is that
                if (content is string)
                {
                    value = content;
                }
                //if formula
                if (content is Formula)
                {
                    Formula f = content as Formula;
                    //retrieve Evaluate's result
                    value = f.Evaluate(Lookup);
                }
            }
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
            //if name is an empty string
            if (name == "" )
            {
                return false;
            }
            //check if name is valid acording to spreadsheet's validation
            if (!spreadsheetValid(name))
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
        /// Checks if a cell already exists and if it's contents is a formula. Looks at the formula and deletes
        /// all variables that it contains in dependencies dependees.
        /// </summary>
        /// <param name="name"></param>
        private void ifCellExists(string name)
        {
            //check if cell name already exists
            if (cellGraph.ContainsKey(name))
            {
                //check if the content is a formula
                object cellContents = GetCellContents(name);
                if (cellContents is Formula)
                {
                    Formula f = (Formula)cellContents;
                    foreach (string variable in f.GetVariables())
                    {
                        cellDependents.RemoveDependency(variable, name);
                    }
                }
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
        /// Set the cell's contents to be a string and return all dependents
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        protected override ISet<String> SetCellContents(String name, String text)
        {
            HashSet<string> dependeesDependents = new HashSet<string>();
            Cell newCell = new Cell(text);
            //update cellGraph
            addToCellGraph(name, newCell);
            //get all direct and indirect dependents
            dependeesDependents = new HashSet<string>(GetCellsToRecalculate(name));
            return dependeesDependents;
        }
    }
}
