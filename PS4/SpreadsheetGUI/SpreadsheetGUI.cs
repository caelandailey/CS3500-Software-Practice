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
            if (value == "")
            {
                ss.SetValue(col, row, DateTime.Now.ToLocalTime().ToString("T"));
                ss.GetValue(col, row, out value);
                MessageBox.Show("Selection: column " + col + " row " + row + " value " + value);
            }
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
            col += 64;
            cellName.Text = (char)col + row + ":";
        }

        

    }
}
