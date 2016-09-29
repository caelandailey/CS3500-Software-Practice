using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tip_Calculator
{
    public partial class totalBillLabel : Form
    {
        public totalBillLabel()
        {
            InitializeComponent();
        }

        private void totalBox_TextChanged(object sender, EventArgs e)
        {
            //declarations
            string billstr = totalBox.Text;
            double bill = 0;
            double tip = 1;

            if(billstr == "")
            {
                totalAmountBox.Text = "0.00";
                return;
            }
            //atempt
            if (Double.TryParse(billstr, out bill))
            {
                if(percentBox.Text == "")
                {
                    totalAmountBox.Text = bill.ToString("N2");
                    return;
                }
            }
            else
            {
                totalAmountLabel.Text = "Error";
                return;
            }

            totalAmountBox.Text = bill.ToString("N2");
            return;
        }
    }
}
