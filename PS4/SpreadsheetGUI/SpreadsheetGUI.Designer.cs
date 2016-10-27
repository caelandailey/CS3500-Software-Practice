namespace SS
{
    partial class SpreadsheetGUI
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
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.File = new System.Windows.Forms.ListBox();
            this.cellName = new System.Windows.Forms.TextBox();
            this.cellValue = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.enterButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.Location = new System.Drawing.Point(1, 26);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(613, 344);
            this.spreadsheetPanel1.TabIndex = 0;
            // 
            // File
            // 
            this.File.FormattingEnabled = true;
            this.File.Items.AddRange(new object[] {
            "Open ",
            "Save",
            "Close"});
            this.File.Location = new System.Drawing.Point(1, -3);
            this.File.Name = "File";
            this.File.Size = new System.Drawing.Size(53, 30);
            this.File.TabIndex = 1;
            // 
            // cellName
            // 
            this.cellName.Location = new System.Drawing.Point(111, 0);
            this.cellName.Name = "cellName";
            this.cellName.Size = new System.Drawing.Size(46, 20);
            this.cellName.TabIndex = 2;
            this.cellName.Text = "A1:";
            // 
            // cellValue
            // 
            this.cellValue.Location = new System.Drawing.Point(154, 0);
            this.cellValue.Name = "cellValue";
            this.cellValue.Size = new System.Drawing.Size(123, 20);
            this.cellValue.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(274, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(209, 20);
            this.textBox1.TabIndex = 4;
            // 
            // enterButton
            // 
            this.enterButton.Location = new System.Drawing.Point(489, 0);
            this.enterButton.Name = "enterButton";
            this.enterButton.Size = new System.Drawing.Size(61, 23);
            this.enterButton.TabIndex = 5;
            this.enterButton.Text = "Enter";
            this.enterButton.UseVisualStyleBackColor = true;
            // 
            // SpreadsheetGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 382);
            this.Controls.Add(this.enterButton);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cellValue);
            this.Controls.Add(this.cellName);
            this.Controls.Add(this.File);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Name = "SpreadsheetGUI";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.ListBox File;
        private System.Windows.Forms.TextBox cellName;
        private System.Windows.Forms.TextBox cellValue;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button enterButton;
    }
}

