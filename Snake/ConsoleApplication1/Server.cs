// Server for the game Snake. Handles the server actions of the snake game. All game models are stored inside of 'World'
// Takes in an XML file with the settings of the game. 
// Creates a world with those settings.
//
// Created by Caelan Dailey and Karina Biancone 12/8/2016 
// cs 3500 ps8
// 
//
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Xml;
using Newtonsoft.Json;

namespace SnakeGame
{
    /// <summary>
    /// Is the server for the game SNAKE. The snake server determines of the snake collides or eats food. The server sends dead food. The server sends dead snakes. 
    /// The server determines the LAWS of the client world. The client controls the direction of the snake while the server determines what it can do when its in certain positions.
    /// </summary>
    class Server
    {
        // Server properties

        private List<SocketState> clients; // Tracks the list of clients connected to the server
        private int clientCount; // Tracks the amount of clients. More accurate than list.count. Easier to increment, ect.
        private object clientLock = new object(); // Lock for each client. Don't want to modify client when going through clients.

        // Snake properties

        private Dictionary<int, int> snakeDirection; // Direction of each snake head
        private Dictionary<int, int> updateDirection; // Track the new direction and set to it the snake direction at the END of the frame.
        private object directionLock = new object(); // Lock direction. Don't want to modify it while chaning it.

        // World Properties

        // These values are set when the XML with the settings are read in at server startup
        private int worldHeight;
        private int worldWidth;
        private int frameRate;
        private int foodDensity;
        private double recycleRate;
        private int startingSnakeLength;
        private int gameMode;
        World world;

        const string filename = "..\\..\\..\\settings.xml"; // File location for XML settings

        /// <summary>
        /// Right when the server is started this program runs. It creates a server object, adds an event for server disconnect and starts the server.
        /// Starting the server starts the TCP listener that listens for sockets
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Server server = new Server(); // Create server object
            Networking.OnDisconnect += server.callMeForException;  // Handle disconnects
            server.StartServer(); // Start server listener

            Console.Read(); // Open server window to see outputs such as connections
        }

        /// <summary>
        /// This is the server object. Creates the client list. Reads the xml. Sets the client count to 0. Create world with settings. 
        /// Create timer. 
        /// Timer calls method every framerate. The update method updates the WORLD on every frame. 
        /// </summary>
        public Server()
        {
            clients = new List<SocketState>();
            readXML(filename); // Read world settings
            clientCount = 0;
            world = new World(worldWidth, worldHeight, recycleRate, startingSnakeLength, gameMode);
            Timer timer = new Timer(frameRate);
            timer.Elapsed += updateWorld; // Set timer action
            timer.AutoReset = true; // Timer resets
            timer.Start(); // Starts timer

            snakeDirection = new Dictionary<int, int>(); // Create snake direction
            updateDirection = new Dictionary<int, int>(); // Create list to update directions. Set snakeDirection to updateDirection at end of frame
        }

        /// <summary>
        /// Updates the game board for one frame.
        /// Moves snakes forward in their direction
        /// Eats food, replenishes food
        /// Determines if snakes die, and recycles them
        /// Used by the server only
        /// 
        /// Confusing method with lots of code. Summed up it does..
        /// Update snake direction. Update snake position. Loop through food, track food. Loop through snakes, track snakes. Send food and snakes. Remove dead food and snakes. Add food.
        /// </summary>
        private void updateWorld(Object source, ElapsedEventArgs e)
        {
            lock (clientLock) // Lock client. Don't want to make changes while accessing it
            {
                //To list to create a copy of the list while iterating through it. SLOWER since copying the entire list, but less bugs.
                foreach (KeyValuePair<int, int> direction in updateDirection.ToList()) // Loop through directions to update. If multiple direction requests in 1 frame. Uses last request
                {
                    lock (directionLock) // Lock direction
                    {
                        snakeDirection[direction.Key] = direction.Value; // Set direction to new direction
                    }
                }
                StringBuilder jsonWorld = new StringBuilder(); // Store values to send
                List<Point> deadFood = new List<Point>(); // Store food to remove later
                List<int> deadSnakes = new List<int>(); // Store snakes to remove later

                // Loop through list of snakes. Uses list to copy it while going through it. Less bugs, but slower.
                foreach (KeyValuePair<int, Snake> snake in world.snakes.ToList()) // Can update snakes since direction is updated.
                {
                    world.MoveSnake(snake.Value, snakeDirection[snake.Key]); // Move every snake

                    jsonWorld.Append(JsonConvert.SerializeObject(snake.Value)); // Add snake to object to send later
                    jsonWorld.Append('\n'); // After every object add this. Client knows its end of object

                    if (snake.Value.vertices.Last().x == -1) // If dead snake
                    {
                        deadSnakes.Add(snake.Key); // Add to list of dead snakes to add later
                    }
                }

                // Loop through list of foods to add or remove
                foreach (KeyValuePair<Point, Food> food in world.foodPoint.ToList())
                {
                    jsonWorld.Append(JsonConvert.SerializeObject(food.Value)); // Add to object that sends this later
                    jsonWorld.Append('\n');
                    if (food.Value.loc.x == -1) // If dead
                    {
                        deadFood.Add(food.Key); // Add to list to remove dead food
                    }
                }

                if (jsonWorld.Length > 2) // Send method also adds '\n' remove that last one or there's an error
                {
                    jsonWorld.Remove(jsonWorld.Length - 1, 1);
                }

                // Loop through clients and send info to each of them
                foreach (SocketState socketState in clients.ToList())
                {
                    Networking.Send(socketState, jsonWorld.ToString());
                }

                // Remove deadFood
                foreach (Point point in deadFood)
                {
                    world.removeFoodPoint(point);

                }

                //Remove snakes
                foreach (int ID in deadSnakes)
                {
                    world.RemoveSnake(ID); // Remove snake
                    lock (directionLock)
                    {
                        snakeDirection.Remove(ID); // Remove from direction list
                    }
                }

                world.createFood(foodDensity); // At end of all this create food
            }
           
        }

        /// <summary>
        /// Start accepting Tcp sockets connections from clients
        /// </summary>
        public void StartServer()
        {
            Networking.ServerAwaitingClientLoop(HandleNewClient); // Wait to get client
        }

        /// <summary>
        /// A callback for invoking when a socket connection is accepted
        /// </summary>
        /// <param name="ar"></param>
        private void HandleNewClient(SocketState socket)
        {
            socket.callMe = ReceivePlayerName; // Got contact from a client

            Networking.GetData(socket); // Wait for more information
        }

        /// <summary>
        /// Make new snake
        /// Create unique id
        /// Change callback to a method that handles direction requests
        /// set socket states id. Equa to unique id
        /// send id and world height/width
        /// data is unique id, world height and width 
        /// add socket to list of client sockets
        /// Then ask for data
        /// </summary>
        /// <param name="state"></param>
        private void ReceivePlayerName(SocketState state)
        {
            ProcessName(state); // Process the player name by MAKING the snake etc. THEN add to client later in this method.

            state.callMe = handleDirectionRequests; // Change callback to handle requests

            Networking.Send(state, clientCount + "\n" + worldWidth + "\n" + worldHeight + "\n"); // Send start information

            lock (clientLock) // Lock client since changing it
            {
                state.ID = clientCount; // set state id

                clients.Add(state); // add to client list

                clientCount++; // increment clientcount
            }

            Networking.GetData(state); // Start listening for more parts of a message, or more new messages
        }

        private void handleDirectionRequests(SocketState state)
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

                int direction = (Int32.Parse(p.ElementAt(1).ToString())); // Get direction. Element at 1 since it's in the format '(1)\n'

                int headDirection = 0;

                lock (directionLock) // Accessing direction set lock
                {
                    if (snakeDirection.ContainsKey(state.ID))
                    {
                        headDirection = snakeDirection[state.ID]; // Snakes head direction
                    }
                }

                bool setDirection = true;

                switch (direction) // Check if direction is not the opposite of current direction. Can't go the opposite direction. Impossible
                {
                    case 1:
                        if (headDirection == 3)
                        {
                            setDirection = false;
                        }
                        break;
                    case 2:
                        if (headDirection == 4)
                        {
                            setDirection = false;
                        }
                        break;
                    case 3:
                        if (headDirection == 1)
                        {
                            setDirection = false;
                        }
                        break;
                    case 4:
                        if (headDirection == 2)
                        {
                            setDirection = false;
                        }
                        break;
                }

                if (setDirection) // If not opposite direction update the direction
                {
                    lock (directionLock)
                    {
                        updateDirection[state.ID] = direction; // Set update direction. REAL direction will be updated in world update on the frame.
                    }
                }

                byte[] messageBytes = Encoding.UTF8.GetBytes(p);

                // Remove it from the SocketState's growable buffer
                state.sb.Remove(0, p.Length);

                // Start listening for more parts of a message, or more new messages
                Networking.GetData(state);
            }
        }

        /// <summary>
        /// Called when a client disconnects. Takes in state and removes state from client list
        /// </summary>
        /// <param name="state"></param>
        private void callMeForException(SocketState state)
        {
            lock (clientLock) // Editing information. lock it
            {
                clients.Remove(state);
            }
        }
        /// <summary>
        /// Given the data that has arrived so far, 
        /// potentially from multiple receive operations, 
        /// determine if we have enough to make a complete message,
        /// and process it (print it).
        /// </summary>
        /// <param name="sender">The SocketState that represents the client</param>
        private void ProcessName(SocketState sender)
        {
            string totalData = sender.sb.ToString();

            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            // Loop until we have processed all messages.
            // We may have received more than one.
            foreach (string p in parts)
            {
                if (p.Length == 0)
                    continue;

                if (p[p.Length - 1] != '\n')
                    break;

                int direction = world.createSnake(p.Substring(0, p.Length - 1), clientCount); // Create snake and set direction. Create Snake returns the randomly created direction

                lock (directionLock) // Add direction to list. Add lock since changing direction.
                {
                    snakeDirection[clientCount] = direction;
                }

                sender.sb.Remove(0, p.Length); // Removed since processed
            }
        }

        /// <summary>
        /// Reads the XML file. The XML file holds the settings of the game. 
        /// </summary>
        /// <param name="filename"></param>
        private void readXML(string filename)
        {
            using (XmlReader r = XmlReader.Create(filename))
            {
                while (r.Read())
                {
                    if (r.IsStartElement())
                    {
                        switch (r.Name)
                        {
                            case "BoardWidth": // World width
                                r.Read();
                                worldWidth = Convert.ToInt32(r.Value);
                                break;
                            case "BoardHeight": // World height
                                r.Read();
                                worldHeight = Convert.ToInt32(r.Value);
                                break;
                            case "MSPerFrame": // World frame rate
                                r.Read();
                                frameRate = Convert.ToInt32(r.Value);
                                break;
                            case "FoodDensity": // Food per snake
                                r.Read();
                                foodDensity = Convert.ToInt32(r.Value);
                                break;
                            case "SnakeRecycleRate": // Food made per dead snake
                                r.Read();
                                recycleRate = Convert.ToDouble(r.Value);
                                break;
                            case "StartingSnakeLength": // Starting length of each snake
                                r.Read();
                                startingSnakeLength = Convert.ToInt32(r.Value);
                                break;
                            case "GameMode": // Game mode. If set to 1 then the bonus game mode is on.
                                r.Read();
                                gameMode = Convert.ToInt32(r.Value);
                                break;
                        }
                    }
                }
            }
        }
    }
}
