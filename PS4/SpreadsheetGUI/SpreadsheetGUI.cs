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

            cellValue.Text = spreadsheet.GetCellValue(cellName.Text).ToString(); // Set the value textBox to the value
            cellContents.Text = spreadsheet.GetCellContents(cellName.Text).ToString(); // Set the contents textBox to the value

            cellContents.Select(cellContents.Text.Length, 0); // Set the contents textBox cursor to the end of the contents
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
            cellContents.Focus(); // Set the foucs to the contents textBox
        }


        /// <summary>
        /// Closes out of the spreadsheet after close button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
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
                else if (resultBox == DialogResult.No) // If user doesn't want to save before closing
                {
                    Close(); // End program
                }
                // No if statement for close since pressing it already closes the form
            }
            else
            {
                Close(); // If no changes, then no need to save. Close program.
            }
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
                catch (Exception error) // If an error throw message 
                {
                    MessageBox.Show(error.Message); // Show error message
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
                    spreadsheet.Save(saveFileDialog.FileName); // Take in the filepath and save it to the path
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message); // Show error message
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
                updateCells(spreadsheet.SetContentsOfCell(cellName.Text, cellContents.Text)); // Set contents and then update cells
            }
            catch (Exception ex) // If there's an error
            {
                MessageBox.Show(ex.Message); // Show error message
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
                spreadsheetPanel1.SetValue(getColumn(t), getRow(t), spreadsheet.GetCellValue(t).ToString());
            }
        }
     

        /// <summary>
        /// Helper method that takes in a cell name and returns the row
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int getRow(String name)
        {
            return (Convert.ToInt32(name.Substring(1, name.Length-1)) - 1); // Subtract 1 since the panel starts at 0,0 NOT at 1,1
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

            cellContents.Text = ""; // Reset content box
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
            string message = "This program is a spreadsheet. You can add formulas, numbers or text to a cell."; // Help info
            MessageBoxButtons button = MessageBoxButtons.OK; // Set buttons
            DialogResult helpBox; // Initialize dialog object

            helpBox = MessageBox.Show(message,"", button); // Show message with properties
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
                spreadsheetPanel1.SetSelection(col-1, row );
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
    }
    
}
