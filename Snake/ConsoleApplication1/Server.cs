using SnakeGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace SnakeGame
{
    class Server
    {
        private List<SocketState> clients;
        private TcpListener listener;
        private int clientCount;
        private object clientLock = new object();
        private Dictionary<int, int> snakeDirection;

        private int worldHeight = 150;
        private int worldWidth = 150;
        private int frameRate = 33;
        private int foodDensity = 10;

        private int snakeCount = 0;
        private double snakeRecycle;
        World world;

        static void Main(string[] args)
        {
            Server server = new Server();

            server.StartServer();



            Console.Read();
        }

        public Server()
        {

            clients = new List<SocketState>();
            //readXML("settings"); //make a setting xml
            clientCount = 0;
            world = new World(worldWidth,worldHeight);
            Timer timer = new Timer(100);
            timer.Elapsed += updateWorld;
            timer.AutoReset = true;
            timer.Start();

            snakeDirection = new Dictionary<int, int>();
        }

        /// <summary>
        /// Updates the game board for one frame.
        /// Moves snakes forward in their direction
        /// Eats food, replenishes food
        /// Determines if snakes die, and recycles them
        /// Used by the server only
        /// </summary>
        private void updateWorld(Object source, ElapsedEventArgs e)
        {
            lock (clientLock)
            {
                foreach (SocketState socketState in clients)
                {
                    for (int i = world.snakes.Count - 1; i >= 0; i--)
                    {
                        world.createFood(foodDensity);
                        world.MoveSnake(world.snakes[socketState.ID], snakeDirection[socketState.ID]);
                        

                        //world.moveSnake(world.snakes[socketState.ID]);
                        Networking.Send(socketState.theSocket, JsonConvert.SerializeObject(world.snakes[socketState.ID]));
                    }

                    foreach (KeyValuePair<Point, Food> food in world.foodPoint)
                    {
                        Networking.Send(socketState.theSocket, JsonConvert.SerializeObject(food.Value));
                    }
                }
            }
        }


        /// <summary>
        /// Start accepting Tcp sockets connections from clients
        /// </summary>
        public void StartServer()
        {
            Console.WriteLine("Server waiting for client");

            // start timer

            Networking.ServerAwaitingClientLoop(HandleNewClient);
        }

        /// <summary>
        /// A callback for invoking when a socket connection is accepted
        /// </summary>
        /// <param name="ar"></param>
        private void HandleNewClient(SocketState socket)
        {
            Console.WriteLine("Contact from client");

            //  Socket s = listener.EndAcceptSocket(ar);

            // Save the socket in a SocketState, 
            // so we can pass it to the receive callback, so we know which client we are dealing with.

            //SocketState newClient = (SocketState)ar.AsyncState;

            socket.callMe = ReceivePlayerName;
            // Can't have the server modifying the clients list if it's braodcasting a message.
            ///lock (clients)
            //{
            //clients.Add(newClient);

            //}

            // Start listening for a message
            // When a message arrives, handle it on a new thread with ReceiveCallback
            //                                  the buffer          buffer offset        max bytes to receive                         method to call when data arrives    "state" object representing the socket
            //newClient.theSocket.BeginReceive(newClient.messageBuffer, 0, newClient.messageBuffer.Length, SocketFlags.None, , newClient);
            Networking.GetData(socket);
            // Continue the "event loop" that was started on line 53
            // Start listening for the next client, on a new thread
            //listener.BeginAcceptSocket(ConnectionRequested, null);


            //Networking.ServerAwaitingClientLoop(HandleNewClient);
        }

        /// Make new snake
        /// Create unique id
        /// Change callback to a method that handles direction requests
        /// set socket states id. Equa to unique id
        /// send id and world height/width
        /// data is unique id, world height and width 
        /// add socket to list of client sockets
        /// Then ask for data
        private void ReceivePlayerName(SocketState state)
        {

            // MAKE SNAKE

            //add fifteen to either the x or the y

            ProcessName(state);

            state.callMe = handleDirectionRequests; // change callback to handle requests

            Networking.Send(state.theSocket, clientCount + "\n" + worldWidth + "\n" + worldHeight + "\n");
            lock (clientLock)
            {
                state.ID = clientCount; // set state id

                clients.Add(state); // add to client list

                clientCount++; // increment clientcount
            }

            // Start listening for more parts of a message, or more new messages
            Networking.GetData(state);

            //state.theSocket.BeginReceive(state.messageBuffer, 0, state.messageBuffer.Length, SocketFlags.None, ReceiveCallback, state); // Ask for more info
            // Dont need since processmessage asks to receive more info

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

                Console.WriteLine("received message: \"" + p + "\"");
                int direction = (Int32.Parse(p.ElementAt(1).ToString()));
                int headDirection = snakeDirection[state.ID];
                bool setDirection = true;
                switch (direction)
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

                if (setDirection)
                {
                    snakeDirection[state.ID] = (Int32.Parse(p.ElementAt(1).ToString()));
                }

                byte[] messageBytes = Encoding.UTF8.GetBytes(p);

                // Remove it from the SocketState's growable buffer
                state.sb.Remove(0, p.Length);

                // Start listening for more parts of a message, or more new messages
                Networking.GetData(state);

            }
        }

        /// <summary>
        /// Callback method for when data is received (started from line 80)
        /// </summary>
        /// <param name="ar">The result that includes the "state" parameter from BeginReceive</param>
        //private void ReceiveCallback(IAsyncResult ar)
        //{
        //    // Get the socket state out of the AsyncState
        //    // This is the object that we passed to BeginReceive that represents the socket
        //    SocketState sender = (SocketState)ar.AsyncState;

        //    int bytesRead = sender.theSocket.EndReceive(ar);

        //    // If the socket is still open
        //    if (bytesRead > 0)
        //    {
        //        string theMessage = Encoding.UTF8.GetString(sender.messageBuffer, 0, bytesRead);
        //        // Append the received data to the growable buffer.
        //        // It may be an incomplete message, so we need to start building it up piece by piece
        //        sender.sb.Append(theMessage);
        //        Console.WriteLine("received message: \"" + theMessage + "\"");
        //        // TODO: If we had an "EventProcessor" delagate associated with the socket state,
        //        //       We could call that here, instead of hard-coding this method to call.
        //        ProcessName(sender);
        //    }

        //    // Continue the "event loop" that was started on line 80.
        //    // Start listening for more parts of a message, or more new messages
        //    sender.theSocket.BeginReceive(sender.messageBuffer, 0, sender.messageBuffer.Length, SocketFlags.None, ReceiveCallback, sender);

        //}

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

                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;
                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p[p.Length - 1] != '\n')
                    break;

                Console.WriteLine("received message: \"" + p + "\"");

                lock (clientLock)
                {
                    snakeDirection[clientCount] = world.createSnake(p.Substring(0, p.Length - 1), clientCount);
                }

                //createSnake(JsonConvert.DeserializeObject<string>(p));
                //byte[] messageBytes = Encoding.UTF8.GetBytes(p);

                // Remove it from the SocketState's growable buffer
                sender.sb.Remove(0, p.Length);

            }

        }

        private void SendCallback(IAsyncResult ar)
        {
            SocketState ss = (SocketState)ar.AsyncState;
            // Nothing much to do here, just conclude the send operation so the socket is happy.
            ss.theSocket.EndSend(ar);
        }

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
                            case "BoardWidth":
                                r.Read();
                                worldWidth = Convert.ToInt32(r.Value);
                                break;
                            case "BoardHeight":
                                r.Read();
                                worldHeight = Convert.ToInt32(r.Value);
                                break;
                            case "MSPerFram":
                                r.Read();
                                frameRate = Convert.ToInt32(r.Value);
                                break;
                            case "FoodDensity":
                                r.Read();
                                foodDensity = Convert.ToInt32(r.Value);
                                break;
                            case "SnakeRecycle":
                                r.Read();
                                snakeRecycle = Convert.ToDouble(r.Value);
                                break;

                                //read special case
                        }
                    }
                }
            }
        }
    }

}
