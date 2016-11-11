using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Snake;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

       

        public Form1()
        {
            InitializeComponent();

            
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
        private void moveUpKey()
        {
            // If game has started
            // *** Once the player recieves their player id they can send requests to the server
            // send '1' to the server
            // Note, this should be of the form: "(X)\n"
            // Send "(1)\n" to the server
        }

        /// <summary>
        /// Helper method to handle the code for the move left arrow key
        /// </summary>
        /// <returns></returns>
        private void moveLeftKey()
        {
            // Send "(4)\n" to the server
            Console.WriteLine("hey");
            Console.Read();
        }

        /// <summary>
        /// Helper method to handle the code for the move right key
        /// </summary>
        /// <returns></returns>
        private void moveRightKey()
        {
            // Send "(2)\n" to the server
        }

        /// <summary>
        /// Helper method to handle the down arrow key
        /// </summary>
        /// <returns></returns>
        private void moveDownKey()
        {
            // Send "(3)\n" to the server
        }


        private void connectButton(object sender, EventArgs e)
        {
            Controller.Controller.connectToServer(serverTextBox.Text);
        }


        

    }
}
