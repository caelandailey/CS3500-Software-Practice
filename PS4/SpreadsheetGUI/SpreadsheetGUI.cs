/// Author: Caelan Dailey & Karina Biancone
/// This GUI project uses SpreadsheetPanel to represent a spreadsheet which can hold numbers, strings, and calculate formulas in the cell selected. There is a 
/// help window that explains shortcuts and how to navigate the spreadsheet in order to enter contents in the cell. The file menu strip gives options on opening a new
/// spreadsheet, an existing spreadsheet, saving a spreadsheet, saving a new spreasheet, or simply closing the spreadsheet. The spreadsheet ranges in cells from A-Z and 1-99.
/// 
/// CS 3500 PS6
/// 11/3/2016
///

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SS
{
    public partial class SpreadsheetGUI : Form
    {
        // Create spreadsheet
        private AbstractSpreadsheet spreadsheet;

        //Track the file path, if empty then file has not been saved. Easy way to track if file has been saved before.
        private string filePath;

        // Construct GUI
        public SpreadsheetGUI()
        {
            InitializeComponent(); // Required method for designer support

            spreadsheet = new Spreadsheet(s => true, s => s.ToUpper(), "ps6"); // Create spreadsheet

            spreadsheetPanel1.SelectionChanged += displaySelection; // If there's a selection changed then update selection and value
            spreadsheetPanel1.SelectionChanged += displayValue;
        }

        /// <summary>
        /// Gets the value of the cell selected
        /// </summary>
        /// <param name="ss"></param>
        private void displayValue(SpreadsheetPanel ss)
        {
            int row, col; // Initialize variables. getSelection uses 'out', need to create variables before.
            ss.GetSelection(out col, out row); // Find where we want to display the value

            valueTextBox.Text = spreadsheet.GetCellValue(cellName.Text).ToString(); // Set the value textBox to the value

            //check if the value is a formula error
            checkFormulaError(col, row);

            contentsTextBox.Text = spreadsheet.GetCellContents(cellName.Text).ToString(); // Set the contents textBox to the value
            contentsTextBox.Select(contentsTextBox.Text.Length, 0); // Set the contents textBox cursor to the end of the contents
        }

        /// <summary>
        /// Helper method to check if the cells value is a formula error in order to set the text to invalid error, which is our own text error
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        private void checkFormulaError(int col, int row)
        {
            if (!(spreadsheet.GetCellValue(cellName.Text) is double) && !(spreadsheet.GetCellValue(cellName.Text) is string))
            {
                valueTextBox.Text = "Invalid error.";
                spreadsheetPanel1.SetValue(col, row, "Invalid error.");
            }
        }
        /// <summary>
        /// Shows the cell's name selected
        /// </summary>
        /// <param name="ss"></param>
        private void displaySelection(SpreadsheetPanel ss)
        {
            int row, col; // Initialize row and col for the getSelection 'out'
            ss.GetSelection(out col, out row);

            col += 65; // Increase by 65 since it starts at 0. Col are letters
            row += 1;  // Increase by 1 since it's a number, starts at 0 and we want it at 1.                   

            cellName.Text = (char)col + "" + (int)row; // Set cell name to the row and col
            contentsTextBox.Focus(); // Set the foucs to the contents textBox
        }


        /// <summary>
        /// Closes out of the spreadsheet after close button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Saves a spreadsheet, if it has not been saved yet it will call on save as function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filePath == null) // File not saved before, do save as
            {
                saveAsToolStripMenuItem.PerformClick();
            }
            else // File path exists, save it
            {
                try
                {
                    spreadsheet.Save(filePath);
                }
                catch (Exception) // If an error throw message 
                {
                    string caption = "Save Error";
                    string message = "Error occured when attempting to save the file. Please check that the file is valid";
                    MessageBox.Show(message, caption, MessageBoxButtons.OK); // Show error message
                }
            }
        }

        /// <summary>
        /// Finds a location to save the spreadsheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog(); // Create file dialog

            saveFileDialog.Filter = "All files (*.*)|*.*|sprd files (*.sprd)|*.sprd"; // Set files
            saveFileDialog.FilterIndex = 2; // Set default file
            saveFileDialog.RestoreDirectory = true; // Restore directory before closing

            if (saveFileDialog.ShowDialog() == DialogResult.OK) // If the 'ok' button is pressed
            {
                try
                {
                    filePath = saveFileDialog.FileName;
                    spreadsheet.Save(filePath); // Take in the filepath and save it to the path
                }
                catch (Exception)
                {
                    string caption = "Save Error";
                    string message = "Error occured when attempting to save the file. Please check that the file is valid";
                    MessageBox.Show(message, caption, MessageBoxButtons.OK); // Show error message
                }
            }
        }

        /// <summary>
        /// Takes the text in the contents box and applies it to the spreadsheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enterButton_Click(object sender, EventArgs e)
        {
            int col, row;
            spreadsheetPanel1.GetSelection(out col, out row); // Get selection

            try // If there's an error, we want to catch it
            {
                updateCells(spreadsheet.SetContentsOfCell(cellName.Text, contentsTextBox.Text)); // Set contents and then update cells
            }
            catch (FormatException) // If there's an error
            {
                string caption = "Format Error";
                string message = "There's is an error with the format of the formula.";
                MessageBox.Show(message, caption, MessageBoxButtons.OK); // Show error message
            }
            catch (CircularException)
            {
                string caption = "Circular Dependency Error";
                string message = "There is either an indirect or direct dependency on another cell's value.";
                MessageBox.Show(message, caption, MessageBoxButtons.OK); // Show error message
            }
            catch (InvalidNameException)
            {
                string caption = "Invalid Name Error";
                string message = "The name given is invalid for this spreadsheet.";
                MessageBox.Show(message, caption, MessageBoxButtons.OK); // Show error message
            }
            catch
            {
                string caption = "Invalid Formula";
                string message = "This formula is invalid!";
                MessageBox.Show(message, caption, MessageBoxButtons.OK); // Show error message
            }

            displayValue(spreadsheetPanel1); // Update the value for that specific cell
        }


        /// <summary>
        /// Helper method that takes in a list of cells that need to be updated and updates the values in the GUI
        /// </summary>
        /// <param name="cells"></param>
        private void updateCells(IEnumerable<string> cells)
        {
            foreach (string t in cells)
            {
                int col, row;
                col = getColumn(t);
                row = getRow(t);
                spreadsheetPanel1.SetValue(col, row, spreadsheet.GetCellValue(t).ToString());
                checkFormulaError(col, row);
            }
        }


        /// <summary>
        /// Helper method that takes in a cell name and returns the row
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int getRow(String name)
        {
            return (Convert.ToInt32(name.Substring(1, name.Length - 1)) - 1); // Subtract 1 since the panel starts at 0,0 NOT at 1,1
        }

        /// <summary>
        /// Helper method that takes in a cell name and returns the row
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int getColumn(String name)
        {
            return ((char.ToUpper(name[0]) - 64) - 1); // Subtract 1 since the panel starts at 0,0 NOT at 1,1
        }

        /// <summary>
        /// Opening a file overrides the current window. Replace spreadsheet with new spreadsheet. Check if wanting to save first
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void existingFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (spreadsheet.Changed)
            {
                // Configure message box
                string message = "Want to save your changes?";
                string caption = "Unsaved Changes";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                DialogResult resultBox;

                resultBox = MessageBox.Show(message, caption, buttons); // Display message

                if (resultBox == DialogResult.Yes) // If they want to save
                {
                    saveNewToolStripMenuItem.PerformClick(); // Save it, this method handles if changes have been made or not.
                }
            }

            OpenFileDialog openFileDialog = new OpenFileDialog(); // Create dialog object

            // Initialize dialog properties
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "All files (*.*)|*.*|sprd files (*.sprd)|*.sprd";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK) // User pressed open
            {
                try
                {
                    resetView(spreadsheet.GetNamesOfAllNonemptyCells()); // Reset all cells to empty
                    spreadsheet = new Spreadsheet(openFileDialog.FileName, s => true, s => s.ToUpper(), "ps6"); // Load new spreadsheet values
                    filePath = openFileDialog.FileName;
                    updateCells(spreadsheet.GetNamesOfAllNonemptyCells()); // Update view with new values
                }
                catch (Exception error) // If any errors with loading file
                {
                    MessageBox.Show("Could not read file from disk: " + error.Message);
                }
            }
        }

        /// <summary>
        /// Helper method that resets the view no an empty spreadsheet. Needed for opening a file and overriding the current file. 
        /// Takes in list of cells to reset to empty
        /// </summary>
        /// <param name="cells"></param>
        private void resetView(IEnumerable<string> cells)
        {
            foreach (string t in cells)
            {
                spreadsheetPanel1.SetValue(getColumn(t), getRow(t), ""); // Set to empty
            }

            contentsTextBox.Text = ""; // Reset content box
            spreadsheetPanel1.SetSelection(0, 0); // Reset to default selection
            updateSelection(); // Updated selection in GUI
        }

        /// <summary>
        /// Button that creates a new form. Opens entirely new spreadsheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DemoApplicationContext.getAppContext().RunForm(new SpreadsheetGUI());  // Tell the application context to run the form on the same thread as the other forms
        }

        /// <summary>
        /// Button that opens a help menu that describes the program. Help menu is there incase user gets confused. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpButton_Click(object sender, EventArgs e)
        {
            // Create the help message
            string caption = "Spreadsheet Information";
            string message = "This program is a spreadsheet. You can add formulas, numbers or text to a cell. When you press enter on the keyboard the enter button on the spreadsheet is invoked." + "\n" + "\n" + "Here are some other short keys that might be useful."; // Help info
            string shortcutMessage = "\n" + "Close: F1" + "\n" + "Save: Ctrl+S" + "\n" + "Save as: Crtl+Alt+S" + "\n" + "Open new: Ctrl+N" + "\n" + "Open existing file: Ctrl+O";
            string keyMessage = "\n" + "\n" + "To navigate to a different cell you may use the arrow keys such as down, left, up, right, or you may select a cell with a mouse.";
            string textBoxMessage = "\n" + "\n" + "The text box on the top left shows the value of the selected cell. The text box to the right shows the contents of the selected cell.";
            MessageBoxButtons button = MessageBoxButtons.OK; // Set buttons
            DialogResult helpBox; // Initialize dialog object

            helpBox = MessageBox.Show(message + shortcutMessage + keyMessage + textBoxMessage, caption, button); // Show message with properties
        }

        /// <summary>
        /// Method that proccesses arrow keys. Overrides key since other objects such as scroll bar can mess with it
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up) // If 'up' arrow key is pressed
            {
                moveUpKey(); // Handle work in another method
            }

            if (keyData == Keys.Down)
            {
                moveDownKey();
            }

            if (keyData == Keys.Left)
            {
                moveLeftKey();
            }

            if (keyData == Keys.Right)
            {
                moveRightKey();
            }

            return base.ProcessCmdKey(ref msg, keyData); // If none of the given keys were pressed
        }

        /// <summary>
        /// Helper method to handle the code for the move up key
        /// </summary>
        /// <returns></returns>
        private bool moveUpKey()
        {
            int row, col;
            spreadsheetPanel1.GetSelection(out col, out row); // Get selection

            if (row == 0) // If the row is at the top, then do nothing
            {
                return true;
            }
            else // If not at top then move up 1
            {
                spreadsheetPanel1.SetSelection(col, row - 1);
                updateSelection();
            }
            return true;
        }

        /// <summary>
        /// Helper method to handle the code for the move left arrow key
        /// </summary>
        /// <returns></returns>
        private bool moveLeftKey()
        {
            int row, col;
            spreadsheetPanel1.GetSelection(out col, out row); // Get selection

            if (col == 0) // If all the way to the left, do nothing
            {
                return true;
            }
            else // Space open on the left, move left
            {
                spreadsheetPanel1.SetSelection(col - 1, row);
                updateSelection();
            }
            return true;
        }

        /// <summary>
        /// Helper method to handle the code for the move right key
        /// </summary>
        /// <returns></returns>
        private bool moveRightKey()
        {
            int row, col;
            spreadsheetPanel1.GetSelection(out col, out row); // Get selection

            if (col == 25) // If all the way to the right, do nothing. Panel goes from 0-25 (26 spaces)
            {
                return true;
            }
            else // Go right
            {
                spreadsheetPanel1.SetSelection(col + 1, row); 
                updateSelection();
            }
            return true;
        }

        /// <summary>
        /// Helper method to handle the down arrow key
        /// </summary>
        /// <returns></returns>
        private bool moveDownKey()
        {
            int row, col;
            spreadsheetPanel1.GetSelection(out col, out row); // Get selection

            if (row == 98) // If at the bottom do nothing. Panel goes from 0-98 (99 spaces)
            {
                return true;
            }
            else // Go down 
            {
                spreadsheetPanel1.SetSelection(col, row + 1);
                updateSelection();
            }
            return true;
        }

        /// <summary>
        /// Helper method to update the selection GUI. 
        /// Used whenever spreadsheet.panel.setSelection is used since the GUI doesnt update on its own. 
        /// </summary>
        private void updateSelection()
        {
            displaySelection(spreadsheetPanel1); // Update selection GUI
            displayValue(spreadsheetPanel1); // Update value GUI
        }

        /// <summary>
        /// Executes when the form closes. If the spreadsheet has changed at all then it asks the user if they want to save, discard, or cancel. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpreadsheetGUI_FormClosing(object sender, FormClosingEventArgs e )
        {
            if (spreadsheet.Changed) // If the spreadsheet as been changed or edited at all
            {
                // Initializes the variables to pass to the MessageBox.Show method.
                string message = "Want to save your changes?";
                string caption = "Unsaved Changes";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel; // Sets the buttons on the message
                DialogResult resultBox; // Initialize dialog menu

                resultBox = MessageBox.Show(message, caption, buttons); // Show the message

                if (resultBox == DialogResult.Yes) // If yes is pressed, "Yes I want to save before I close"
                {
                    saveNewToolStripMenuItem.PerformClick(); // Save, this method handles if file has saved before              
                }
                else if (resultBox == DialogResult.Cancel) // If user pressed cancel
                {
                    e.Cancel = true; // Set cancel to true, cancel the closing
                }
            }
        }
    }
}
