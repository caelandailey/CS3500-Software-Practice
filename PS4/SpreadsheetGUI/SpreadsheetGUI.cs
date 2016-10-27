using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SS
{
    public partial class SpreadsheetGUI : Form
    {
        public SpreadsheetGUI()
        {
            InitializeComponent();

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
            ss.GetSelection(out col, out row);
            ss.GetValue(col, row, out value);
            
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
            
            cellName.Text = (char)col + "" + (int)row + ":";
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
                //pop up window to tell user the spreadsheet has not been saved
            Close();
        }

        private void saveNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if the file has been saved already
                //call on the spreadsheet to save the file
            //create a new save window
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //invoke a new save window
        }
    }
}
