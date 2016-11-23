/// Caelan Dailey 
/// Karina Biancone
/// 11/22/2016
/// Snake Game
/// CS 3500 



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

    /// <summary>
    /// This class is the GUI of the snake game.
    /// It controls how the snake game is viewed.
    /// It controls input and transports that to the network or the world database
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Variables to track the form information
        /// </summary>
        private string playerName;
        private int worldWidth;
        private int worldHeight;
        private int playerID;
        private bool connected = false;
        private Dictionary<string, int> snakeScores; // Holds snake scores
        private Dictionary<string, Color> snakeColor; // Holds snake colors

        public Form1()
        {
            InitializeComponent();
            // this.Size = (large enough to hold all buttons, panels, etc)
            this.Size = new Size(1000, 300);
            snakeScores = new Dictionary<string, int>();
            snakeColor = new Dictionary<string, Color>();
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
            if (keyData == Keys.Up || keyData == Keys.NumPad8 || keyData == Keys.W) // If 'up' arrow key is pressed
            {
                moveUpKey(); // Handle work in another method
            }

            if (keyData == Keys.Down || keyData == Keys.NumPad2 || keyData == Keys.S)
            {
                moveDownKey();
            }

            if (keyData == Keys.Left || keyData == Keys.NumPad4 || keyData == Keys.A)
            {
                moveLeftKey();
            }

            if (keyData == Keys.Right || keyData == Keys.NumPad6 || keyData == Keys.D)
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

            //Disable since conneted
            nameTextBox.Enabled = false;
            serverTextBox.Enabled = false;
  
            // Get player name
            playerName = nameTextBox.Text;
            connectButton.Enabled = false;

            //Connect
            connectToServer(serverTextBox.Text);
        }

        /// <summary>
        /// Connects to the server
        /// </summary>
        /// <param name="serverIP"></param>
        public void connectToServer(string serverIP)
        {
            Networking.ConnectToServer(FirstContact, serverIP);
        }

        /// <summary>
        /// The first contact with server. Send the playername. 
        /// </summary>
        /// <param name="state"></param>
        public void FirstContact(SocketState state)
        {
            state.callMe = ReceieveStartup;
            Networking.Send(state.theSocket, playerName);
        }

        /// <summary>
        /// Recieves the startup. Server sends startup information to client
        /// </summary>
        /// <param name="state"></param>
        public void ReceieveStartup(SocketState state)
        {
            ProcessStartup(state);
            state.sb.Clear(); //Warning: This assumes ReceiveStartup is only called once

            state.callMe = ReceiveWorld;
            Networking.GetData(state);
        }

        /// <summary>
        /// Receieves the world. Server sends world information to client
        /// </summary>
        /// <param name="state"></param>
        public void ReceiveWorld(SocketState state)
        {
            connected = true;
            ProcessWorld(state);

            Networking.GetData(state);
        }

        /// <summary>
        /// Helper method to send message to server
        /// </summary>
        /// <param name="data"></param>
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
                    Invoke(new MethodInvoker(() => worldPanel.width = worldWidth)); // Adjust world sizes to new size from server
                    Invoke(new MethodInvoker(() => worldPanel.height = worldHeight));

                    // set scoreboard size
                    Invoke(new MethodInvoker(() => scoreBoard.Size = new Size(500, worldHeight*World.pixelsPerCell)));

                    // if the panel is too big for the form then update the form. 
                    Invoke(new MethodInvoker(() => this.Size = new Size(scoreBoard.Width+worldWidth*World.pixelsPerCell+100, worldPanel.Location.Y + worldHeight * World.pixelsPerCell+ 50))); // add size of score size
                    
                    // Set the size of the drawing panel to match the world
                    Invoke(new MethodInvoker(() => worldPanel.Size = new Size(worldPanel.width * World.pixelsPerCell, worldPanel.height * World.pixelsPerCell)));
                }

                //Update position of startup information
                position++;
            }
        }

        /// <summary>
        /// Helper method to update the scores of the scoreboard
        /// </summary>
        private void UpdateScore()
        {
            Invoke(new MethodInvoker(() => scoreBoard.Clear())); // Clear scoreboard then redraw
            foreach (KeyValuePair<String, int> scores in snakeScores) // Loop through scores
            {
                string score = scores.Key + ": " + scores.Value; // Set up label
                Invoke(new MethodInvoker(() => scoreBoard.AppendText(score + "\n")));
                Invoke(new MethodInvoker(() => scoreBoard.Find(score)));
                Invoke(new MethodInvoker(() => scoreBoard.SelectionColor = snakeColor[scores.Key]));
            }
        }

        /// <summary>
        /// Proccess the information from the server, this includes the snake and food. Not startup information
        /// </summary>
        /// <param name="state"></param>
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

                // Parse object
                JObject obj = JObject.Parse(p);
                JToken someProp = obj["vertices"];
                if (someProp != null) // we are dealing with a snake
                {

                    Snake snake = JsonConvert.DeserializeObject<Snake>(p); // Get object

                    Invoke(new MethodInvoker(() =>
                    {
                        try
                        {
                            snakeScores[snake.name] = snake.getSnakeLength(); //Get Length
                        }
                        catch { }
                    }
            ));


                    if (!snakeColor.ContainsKey(snake.name)) // Set color
                    {
                        Random random = new Random();
                        Color color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                        snakeColor[snake.name] = color;

                    }
                    // Set color to snake.color for later when drawn in world
                    Invoke(new MethodInvoker(() =>
                    {
                        try
                        {
                            snake.color = snakeColor[snake.name];
                        }
                        catch { }
                    }
                    ));

                    if (snake.vertices.Last().x == -1) // If snake is dead
                    {
                        // Remove from database
                        Invoke(new MethodInvoker(() => worldPanel.RemoveSnake(snake.ID)));
                        Invoke(new MethodInvoker(() => snakeColor.Remove(snake.name)));
                        Invoke(new MethodInvoker(() => snakeScores.Remove(snake.name)));
                    }
                    else
                    {
                        Invoke(new MethodInvoker(() => worldPanel.AddSnake(snake)));
                    }

                    // Update scores
                    UpdateScore();
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
                                    worldPanel.AddFood(food); // add food, inside invoke since on another thread. In try since errors
                                }
                                catch { }
                            }
                              ));
                        }
                        catch { }

                    }
                }

                if (state.sb.Length >= p.Length) // remove from state
                {
                    state.sb.Remove(0, p.Length);
                }

                // Try catch
                try
                {
                    Invoke(new MethodInvoker(() => worldPanel.Invalidate())); // Invalidate causes the panel to redraw
                }
                catch { }
            }
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            String message = "This game is call Snake. Snake around and get food and grow bigger. Be careful of the other snakes and walls. The more you grow the bigger your score is. ";
            message += "Press the arrow keys to move around. You may also press w,a,s,d to move around and 8,4,2,and 6 on the keypad to move around.";
            message += "Name your snake in order to join. This game requires a server to play on.";
            MessageBox.Show(message);

        }
    }
}