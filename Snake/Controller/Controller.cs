using Snake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Snake
{
    public class Controller
    {
        

        

        SocketState server;

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
            //(extract data from state)
            state.callMe = ReceiveWorld;
            Networking.GetData(state);
        }

        public static void ReceiveWorld(SocketState state)
        {

        }
    }
}
