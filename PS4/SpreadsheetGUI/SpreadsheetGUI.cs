using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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

        // Deals with the New menu
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same
            // thread as the other forms.
            //DemoApplicationContext.getAppContext().RunForm(new Form1());
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if changed is true
            if (spreadsheet.Changed)
            {
                //pop up window to tell user the spreadsheet has not been saved
            }
                
            Close();
        }

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

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "All files (*.*)|*.*|sprd files (*.sprd)|*.sprd";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;


            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    //save the file path
                    filePath = saveFileDialog1.FileName;
                    //save in spreadsheet
                    spreadsheet.Save(filePath);
                    // Code to write the stream goes here.
                    myStream.Close();
                }
            }
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            int col, row;
            spreadsheetPanel1.GetSelection(out col, out row);
            spreadsheetPanel1.SetValue(col, row, cellContents.Text);
            //add the contents to the spreadsheet graph
                //spreadsheet.SetCellContents(cellContent.Text);
            //update value?
                
            cellContents.Text = "";
            displayValue(spreadsheetPanel1);
        }


    }
}
