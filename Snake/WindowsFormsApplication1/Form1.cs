using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private string playerName;
        private int worldWidth;
        private int worldHeight;
        private int playerID;
        private bool connected = false;
        

        //private World world;
        public Form1()
        {
            InitializeComponent();
            // TODO: We would also need to update this form's size to expand or shrink to fit the panel
            // this.Size = (large enough to hold all buttons, panels, etc)
            this.Size = new Size(1000, 1000);
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

            sendMessage("(1)");
            
        }

        /// <summary>
        /// Helper method to handle the code for the move left arrow key
        /// </summary>
        /// <returns></returns>
        private void moveLeftKey()
        {
            // Send "(4)\n" to the server

            sendMessage("(4)");
           
        }

        /// <summary>
        /// Helper method to handle the code for the move right key
        /// </summary>
        /// <returns></returns>
        private void moveRightKey()
        {
            // Send "(2)\n" to the server

            sendMessage("(2)");
         
        }

        /// <summary>
        /// Helper method to handle the down arrow key
        /// </summary>
        /// <returns></returns>
        private void moveDownKey()
        {
            // Send "(3)\n" to the server

            sendMessage("(3)");
           
        }

        /// <summary>
        /// Connects to the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectServerButton(object sender, EventArgs e)
        {
            if (serverTextBox.Text == "")
            {
                MessageBox.Show("Please enter a server address");
                return;
            }

            nameTextBox.Select(0, 0); // Deselect the boxes
            serverTextBox.Select(0, 0);
            playerName = nameTextBox.Text;
            connectToServer(serverTextBox.Text);
        }

        public void connectToServer(string serverIP)
        {
            Networking.ConnectToServer(FirstContact, serverIP);
        }

        public void FirstContact(SocketState state)
        {
            state.callMe = ReceieveStartup;
            Networking.Send(state.theSocket, playerName);
        }

        public void ReceieveStartup(SocketState state)
        {
            ProcessStartup(state);
            state.sb.Clear(); //Warning: This assumes ReceiveStartup is only called once

            state.callMe = ReceiveWorld;
            Networking.GetData(state);
        }

        public void ReceiveWorld(SocketState state)
        {
            ProcessWorld(state);

            Networking.GetData(state);
        }

        public void sendMessage(string data)
        {
            Networking.Send(Networking.server.theSocket, data);
        }


        private void ProcessStartup(SocketState state)
        {
            string totalData = state.sb.ToString();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");
            int position = 1;
            // Loop until we have processed all messages.
            // We may have received more than one.

            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;
                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p[p.Length - 1] != '\n')
                    break;

                switch (position)
                {
                    case 1: // Its player name
                        playerID = (Convert.ToInt32(p));
                        break;
                    case 2:
                        worldWidth = (Convert.ToInt32(p));
                        break;
                    case 3:
                        worldHeight = (Convert.ToInt32(p));
                        break;
                }

                

                if (position == 3) // recieved all data
                {
 
                    Invoke(new MethodInvoker(() => worldPanel.width = worldWidth));
                    Invoke(new MethodInvoker(() => worldPanel.height = worldHeight));
                    // May need to change the size of the form 'this.size'

                    // if the panel is too big for the form then update the form. 

                    // Set the size of the drawing panel to match the world
                    Invoke(new MethodInvoker(() => worldPanel.Size = new Size(worldPanel.width * World.pixelsPerCell, worldPanel.height * World.pixelsPerCell)));
                }

                position++;

            }

            
            //Invoke(new MethodInvoker(() => worldPanel.Invalidate()));
            //Invoke(new MethodInvoker(this.Update));
        }
        private void ProcessWorld(SocketState state)
        {

            string totalData = state.sb.ToString();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            // Loop until we have processed all messages.
            // We may have received more than one.

            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;
                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p[p.Length - 1] != '\n')
                    break;



                //Food rebuilt = JsonConvert.DeserializeObject<Food>(p);
                JObject obj = JObject.Parse(p);
                JToken someProp = obj["vertices"];
                if (someProp != null) // we are dealing with a snake
                {

                    Snake snake = JsonConvert.DeserializeObject<Snake>(p);

                    Point deadPoint = new Point(-1, -1);

                    if (snake.vertices.Contains(deadPoint))
                    {
                        Invoke(new MethodInvoker(() => worldPanel.RemoveSnake(snake.ID)));

                    }
                    else
                    {
                        // snake.setColor();
                        Invoke(new MethodInvoker(() => worldPanel.AddSnake(snake)));
                    }
                }
                else // if not its food
                {
                    Food food = JsonConvert.DeserializeObject<Food>(p);

                    Point deadPoint = new Point(-1, -1);

                    if (food.loc.x == -1)
                    {
                        Invoke(new MethodInvoker(() => worldPanel.RemoveFood(food.ID)));

                    }
                    else
                    {
                        Invoke(new MethodInvoker(() => worldPanel.AddFood(food)));

                    }
                }

                if (state.sb.Length >= p.Length)            // DO WE NEED THIS?
                {
                    state.sb.Remove(0, p.Length);
                }



                //try catch
                try
                {
                    Invoke(new MethodInvoker(() => worldPanel.Invalidate()));
                }
                catch { }
                //Invoke(new MethodInvoker(this.Update));

            }

        }
    }
    }