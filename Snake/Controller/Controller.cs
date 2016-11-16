using Snake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Snake
{
    public class Controller
    {
        
        public static void connectToServer(string serverIP)
        {
            Networking.ConnectToServer(FirstContact, serverIP);
        }

        public static void FirstContact(SocketState state)
        {
            state.callMe = ReceieveStartup;
            Networking.Send(state.theSocket, World.playerName);
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

        private static void ProcessMessage(SocketState state)
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
                    World.addSnake(snake);
                }
                else // if not its food
                {
                    Food food = JsonConvert.DeserializeObject<Food>(p);
                    World.addFood(food);
                }

                if (state.sb.Length >= p.Length)            // DO WE NEED THIS?
                {
                    state.sb.Remove(0, p.Length);
                }
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
                        World.playerID = Convert.ToInt32(p);
                        break;
                    case 2:
                        World.worldSizeX = Convert.ToInt32(p);
                        break;
                    case 3:
                        World.worldSizeY = Convert.ToInt32(p);
                        break;
                }
                position++;


            }

        }

    }
}
