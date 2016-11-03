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
        //create spreadsheet
        private AbstractSpreadsheet spreadsheet;

        //a file path
        private string filePath;

        //construct gui
        public SpreadsheetGUI()
        {
            InitializeComponent();

            spreadsheet = new Spreadsheet(s => true, s => s.ToUpper(), "ps6");

            spreadsheetPanel1.SelectionChanged += displaySelection;
            spreadsheetPanel1.SelectionChanged += displayValue;
            
        }

        /// <summary>
        /// Gets the value of the cell selected
        /// </summary>
        /// <param name="ss"></param>
        private void displayValue(SpreadsheetPanel ss)
        {
            int row, col;
            String value;
            String contents;
            ss.GetSelection(out col, out row);
            ss.GetValue(col, row, out contents);

            value = spreadsheet.GetCellValue(cellName.Text).ToString();

            cellValue.Text = value;
            cellContents.Text = spreadsheet.GetCellContents(cellName.Text).ToString();
            cellContents.Select(cellContents.Text.Length, 0);
        }

        /// <summary>
        /// Shows the cell's name selected
        /// </summary>
        /// <param name="ss"></param>
        private void displaySelection(SpreadsheetPanel ss)
        {
            int row, col;
            ss.GetSelection(out col, out row);
            col += 65;
            row += 1;                     
            
            cellName.Text = (char)col + "" + (int)row;
            cellContents.Focus();
        }


        /// <summary>
        /// Closes out of the spreadsheet after close button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if changed is true
            if (spreadsheet.Changed)
            {
                // Initializes the variables to pass to the MessageBox.Show method.
                string message = "Want to save your changes?";
                string caption = "Unsaved Changes";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                DialogResult resultBox;

                // Displays the MessageBox.

                resultBox = MessageBox.Show(message, caption, buttons);

                if (resultBox == DialogResult.Yes)
                {
                    saveNewToolStripMenuItem.PerformClick();                    

                }
                else if (resultBox == DialogResult.No)
                {
                    Close();
                }
                // No if statement for close since pressing it already closes the form
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// Saves a spreadsheet, if it has not been saved yet it will call on save as function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            //if the file has not been saved
            if (filePath == null)
            {
                saveAsToolStripMenuItem.PerformClick();
            }
            //call on the spreadsheet to save the file
            else
            {
                spreadsheet.Save(filePath);
            }           
        }

        /// <summary>
        /// Finds a location to save the spreadsheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "All files (*.*)|*.*|sprd files (*.sprd)|*.sprd";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {               
                    //save the file path
                    filePath = saveFileDialog1.FileName;
                    //save in spreadsheet
                 
                    spreadsheet.Save(filePath);
                    // Code to write the stream goes here.              
                
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
            spreadsheetPanel1.GetSelection(out col, out row);

            try
            {
            updateCells(spreadsheet.SetContentsOfCell(cellName.Text, cellContents.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            //update the value for that specific cell
            displayValue(spreadsheetPanel1);
        }


        /// <summary>
        /// Updates all cell's values
        /// </summary>
        /// <param name="cells"></param>
        private void updateCells(IEnumerable<string> cells)
        {
            foreach (string t in cells)
            {
                spreadsheetPanel1.SetValue(getColumn(t)-1, getRow(t)-1, spreadsheet.GetCellValue(t).ToString());
            }
        }
     

        private int getRow(String name)
        {
            return Convert.ToInt32(name.Substring(1, name.Length-1));
        }

        private int getColumn(String name)
        {
            return char.ToUpper(name[0]) - 64;
        }


        

        private void existingFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (spreadsheet.Changed)
            {
                string message = "Want to save your changes?";
                string caption = "Unsaved Changes";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                DialogResult resultBox;

                // Displays the MessageBox.

                resultBox = MessageBox.Show(message, caption, buttons);

                if (resultBox == DialogResult.Yes)
                {
                    saveNewToolStripMenuItem.PerformClick();
                }
            }

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*|sprd files (*.sprd)|*.sprd";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    resetView(spreadsheet.GetNamesOfAllNonemptyCells()); // Reset all cells to empty
                    spreadsheet = new Spreadsheet(openFileDialog1.FileName, s => true, s => s.ToUpper(), "ps6"); // Load new spreadsheet values
                     updateCells(spreadsheet.GetNamesOfAllNonemptyCells()); // Update view
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void resetView(IEnumerable<string> cells)
        {
            foreach (string t in cells)
            {
                spreadsheetPanel1.SetValue(getColumn(t) - 1, getRow(t) - 1, "");
            }
            cellContents.Text = "";
            spreadsheetPanel1.SetSelection(0, 0); // Need to update where the panel thinks it is, thinks its at old cords. 
            updateSelection();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same
            // thread as the other forms.
            DemoApplicationContext.getAppContext().RunForm(new SpreadsheetGUI());
        }



        private void helpButton_Click(object sender, EventArgs e)
        {
            string message = "This program is a spreadsheet. You can add formulas, numbers or text to a cell.";
           
            MessageBoxButtons button = MessageBoxButtons.OK;
            DialogResult helpBox;

            helpBox = MessageBox.Show(message,"", button);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //capture up arrow key
            if (keyData == Keys.Up)
            {
                moveUpKey();
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

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool moveUpKey()
        {
            int row, col;
            spreadsheetPanel1.GetSelection(out col, out row);

            if (row == 0)
            {
                return true;
            }
            else
            {
                spreadsheetPanel1.SetSelection(col, row - 1);
                updateSelection();
            }
            return true;
        }
        private bool moveLeftKey()
        {
            int row, col;
            spreadsheetPanel1.GetSelection(out col, out row);

            if (col == 0)
            {
                return true;
            }
            else
            {
                spreadsheetPanel1.SetSelection(col-1, row );
                updateSelection();
            }
            return true;
        }

        private bool moveRightKey()
        {
            int row, col;
            spreadsheetPanel1.GetSelection(out col, out row);

            if (col == 25)
            {
                return true;
            }
            else
            {
                spreadsheetPanel1.SetSelection(col + 1, row);
                updateSelection();
            }
            return true;
        }

        private bool moveDownKey()
        {
            int row, col;
            spreadsheetPanel1.GetSelection(out col, out row);

            if (row == 98)
            {
                return true;
            }
            else
            {
                spreadsheetPanel1.SetSelection(col, row + 1);
                updateSelection();
            }
            return true;
        }

        private void updateSelection()
        {
            displaySelection(spreadsheetPanel1);
            displayValue(spreadsheetPanel1);
        }
    }
    
}
