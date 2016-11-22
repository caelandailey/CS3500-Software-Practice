﻿using System;
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
        private static string playerName;
        private static int worldWidth;
        private static int worldHeight;
        private static int playerID;

        private World world;

        // This object represents the world.
        // In this simple demo, the world consists of one dot
        //private DrawWorld world;



        //private DrawingPanel.DrawingPanel panel;


        public Form1()
        {
            InitializeComponent();
            // TODO: We would also need to update this form's size to expand or shrink to fit the panel
            // this.Size = (large enough to hold all buttons, panels, etc)

        }
        private void createWorld()
        {

            world = new World(worldHeight, worldWidth);

            // Set the size of the drawing panel to match the world
            worldPanel.Size = new Size(1000, 1000);

        }


        /// <summary>
        /// This tick event is called 30 times / sec
        /// This method simulates an update coming from the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateFrame()
        {
            // pretend this dot was deserialized from a JSON string sent by the server
            
            // Draw food
            foreach(KeyValuePair<int, Food> food in World.foods)
            {
                DrawFood(food.Value);
            }

            // Draw snake
            foreach (KeyValuePair<int, Snake> snake in World.snakes)
            {
                DrawSnake(snake.Value);
            }

            

            // Cause the panel to redraw
            worldPanel.Invalidate();
        }

        private void DrawSnake(Snake snake)
        {

        }
        private void DrawFood(Food food)
        {
            // world.SetDot(food.loc.x, food.loc.y);
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
            Console.WriteLine("Up key pressed");
        }

        /// <summary>
        /// Helper method to handle the code for the move left arrow key
        /// </summary>
        /// <returns></returns>
        private void moveLeftKey()
        {
            // Send "(4)\n" to the server

            sendMessage("(4)");
            Console.WriteLine("Left key pressed");
            Console.Read();
        }

        /// <summary>
        /// Helper method to handle the code for the move right key
        /// </summary>
        /// <returns></returns>
        private void moveRightKey()
        {
            // Send "(2)\n" to the server

            sendMessage("(2)");
            Console.WriteLine("Right key pressed");
        }

        /// <summary>
        /// Helper method to handle the down arrow key
        /// </summary>
        /// <returns></returns>
        private void moveDownKey()
        {
            // Send "(3)\n" to the server

            sendMessage("(3)");
            Console.WriteLine("Down key pressed");
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

            playerName = nameTextBox.Text;
            connectToServer(serverTextBox.Text);
        }

     
      
      

        public static void connectToServer(string serverIP)
        {
            Networking.ConnectToServer(FirstContact, serverIP);
        }

        public static void FirstContact(SocketState state)
        {
            state.callMe = ReceieveStartup;
            Networking.Send(state.theSocket, playerName);
        }

        public static void ReceieveStartup(SocketState state)
        {
            ProcessStartup(state);
            state.sb.Clear(); //Warning: This assumes ReceiveStartup is only called once

            state.callMe = ReceiveWorld;
            Networking.GetData(state);
        }

        public static void ReceiveWorld(SocketState state)
        {
            ProcessMessage(state);

            Networking.GetData(state);
        }

        public static void sendMessage(string data)
        {
            Networking.Send(Networking.server.theSocket, data);
        }

        private void ProcessMessage(SocketState state)
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
                        world.RemoveSnake(snake.ID);
                    }
                    else
                    {
                        snake.setColor();
                        world.AddSnake(snake);
                    }
                }
                else // if not its food
                {
                    Food food = JsonConvert.DeserializeObject<Food>(p);

                    Point deadPoint = new Point(-1, -1);

                    if (food.loc.x == -1)
                    {
                        world.RemoveFood(food.ID);
                        
                    }
                    else
                    {
                        world.AddFood(food);
                        
                    }
                }

                if (state.sb.Length >= p.Length)            // DO WE NEED THIS?
                {
                    state.sb.Remove(0, p.Length);
                }
                UpdateFrame();
                
            }
        }

        private static void ProcessStartup(SocketState state)
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
                position++;
            }

            

            //World.Update();

        }

        /// <summary>
        /// Helper method for DrawingPanel
        /// Given the PaintEventArgs that comes from DrawingPanel, draw the contents of the world on to the panel.
        /// </summary>
        /// <param name="e"></param>
        public void Draw(PaintEventArgs e)
        {
            using (SolidBrush drawBrush = new SolidBrush(Color.Black))
            {
                // Draw the single dot that represents the world
                Rectangle dotBounds = new Rectangle(5, 5, 20, 20);
                e.Graphics.FillEllipse(drawBrush, dotBounds);



                //Rectangle lineBounds = new Rectangle((point1.X - point2.X) * pixelsPerCell, point1 )
            }


        }

        

    }
}
