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
        private Dictionary<string, int> snakeScores;
        private Dictionary<int, Color> snakeColor;

        //private World world;
        public Form1()
        {
            InitializeComponent();
            // TODO: We would also need to update this form's size to expand or shrink to fit the panel
            // this.Size = (large enough to hold all buttons, panels, etc)
            this.Size = new Size(1000, 1000);
            snakeScores = new Dictionary<string, int>();
            snakeColor = new Dictionary<int, Color>();
        }

        /// <summary>
        /// Method that proccesses arrow keys. Overrides key since other objects such as scroll bar can mess with it
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (connected == false)
            {
                return false;
            }
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

            //nameTextBox.Enabled = false;
            serverTextBox.Enabled = false;
  
            playerName = nameTextBox.Text;
            //connectButton.Enabled = false;
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
            connected = true;
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
                    Invoke(new MethodInvoker(() => scoreBoard.Size = new Size(500, worldHeight*World.pixelsPerCell)));
                    
                    Invoke(new MethodInvoker(() => this.Size = new Size(scoreBoard.Width+worldWidth*World.pixelsPerCell+100, worldPanel.Location.Y + worldHeight * World.pixelsPerCell+ 50))); // add size of score size
                    
                    // Set the size of the drawing panel to match the world
                    Invoke(new MethodInvoker(() => worldPanel.Size = new Size(worldPanel.width * World.pixelsPerCell, worldPanel.height * World.pixelsPerCell)));
                }

                position++;

            }

        }

        private void UpdateScore()
        {
           
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

                    if (!snakeScores.ContainsKey(snake.name))
                    {
                        Invoke(new MethodInvoker(() => snakeScores[snake.name] = snake.getSnakeLength()));
                    }
                    

                    if (!snakeColor.ContainsKey(snake.ID))
                    {
                        Random random = new Random();
                        Color color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                        snakeColor[snake.ID] = color;
                       
                    }
                    Invoke(new MethodInvoker(() => snake.color = snakeColor[snake.ID]));

                    Invoke(new MethodInvoker(() => scoreBoard.Clear()));
                    foreach (KeyValuePair<String, int> scores in snakeScores)
                    {
                        string score = scores.Key + ": " + scores.Value;
                        Invoke(new MethodInvoker(() => scoreBoard.AppendText(score+ "\n")));
                        Invoke(new MethodInvoker(() => scoreBoard.Find(score)));

                        Invoke(new MethodInvoker(() => scoreBoard.SelectionColor = snakeColor[snake.ID]));
                        Console.WriteLine(snake.color);
                        Console.Read();
                    }



                    Point deadPoint = new Point(-1, -1);

                    if (snake.vertices.Contains(deadPoint))
                    {
                        Invoke(new MethodInvoker(() => worldPanel.RemoveSnake(snake.ID)));
                        Invoke(new MethodInvoker(() => snakeColor.Remove(snake.ID)));
                        Invoke(new MethodInvoker(() => snakeScores.Remove(snake.name)));


                    }
                    else
                    {

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
                        try
                        {


                            Invoke(new MethodInvoker(() =>
                            {
                                try
                                {
                                    worldPanel.AddFood(food);
                                }
                                catch { }
                            }
                              ));
                        }
                        catch { }

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
                

            }

        }
    }
    }