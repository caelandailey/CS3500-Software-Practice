namespace Tip_Calculator
{
    partial class totalBillLabel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.totalBox = new System.Windows.Forms.TextBox();
            this.totalAmountBox = new System.Windows.Forms.TextBox();
            this.totalLabel = new System.Windows.Forms.Label();
            this.totalAmountLabel = new System.Windows.Forms.Label();
            this.percentageLabel = new System.Windows.Forms.Label();
            this.percentBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // totalBox
            // 
            this.totalBox.Location = new System.Drawing.Point(188, 37);
            this.totalBox.Name = "totalBox";
            this.totalBox.Size = new System.Drawing.Size(150, 20);
            this.totalBox.TabIndex = 1;
            this.totalBox.TextChanged += new System.EventHandler(this.totalBox_TextChanged);
            // 
            // totalAmountBox
            // 
            this.totalAmountBox.Location = new System.Drawing.Point(188, 199);
            this.totalAmountBox.Name = "totalAmountBox";
            this.totalAmountBox.Size = new System.Drawing.Size(150, 20);
            this.totalAmountBox.TabIndex = 2;
            this.totalAmountBox.Text = "0.00";
            // 
            // totalLabel
            // 
            this.totalLabel.AccessibleName = "Enter Total Bill";
            this.totalLabel.AutoSize = true;
            this.totalLabel.Location = new System.Drawing.Point(82, 44);
            this.totalLabel.Name = "totalLabel";
            this.totalLabel.Size = new System.Drawing.Size(51, 13);
            this.totalLabel.TabIndex = 3;
            this.totalLabel.Text = "Enter Bill:";
            // 
            // totalAmountLabel
            // 
            this.totalAmountLabel.AutoSize = true;
            this.totalAmountLabel.Location = new System.Drawing.Point(82, 199);
            this.totalAmountLabel.Name = "totalAmountLabel";
            this.totalAmountLabel.Size = new System.Drawing.Size(73, 13);
            this.totalAmountLabel.TabIndex = 4;
            this.totalAmountLabel.Text = "Total Amount:";
            // 
            // percentageLabel
            // 
            this.percentageLabel.AutoSize = true;
            this.percentageLabel.Location = new System.Drawing.Point(41, 91);
            this.percentageLabel.Name = "percentageLabel";
            this.percentageLabel.Size = new System.Drawing.Size(92, 13);
            this.percentageLabel.TabIndex = 5;
            this.percentageLabel.Text = "Total Percentage:";
            // 
            // percentBox
            // 
            this.percentBox.Location = new System.Drawing.Point(188, 84);
            this.percentBox.Name = "percentBox";
            this.percentBox.Size = new System.Drawing.Size(150, 20);
            this.percentBox.TabIndex = 6;
            // 
            // totalBillLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 304);
            this.Controls.Add(this.percentBox);
            this.Controls.Add(this.percentageLabel);
            this.Controls.Add(this.totalAmountLabel);
            this.Controls.Add(this.totalLabel);
            this.Controls.Add(this.totalAmountBox);
            this.Controls.Add(this.totalBox);
            this.Name = "totalBillLabel";
            this.Text = "Enter Total Bill:";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox totalBox;
        private System.Windows.Forms.TextBox totalAmountBox;
        private System.Windows.Forms.Label totalLabel;
        private System.Windows.Forms.Label totalAmountLabel;
        private System.Windows.Forms.Label percentageLabel;
        private System.Windows.Forms.TextBox percentBox;
    }
}

