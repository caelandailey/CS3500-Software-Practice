using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    // Should know what to do when when data comes in....
    //(1) Which socket is the data on
    //(2) What previous data has arrived
    //(3) what function to call when data arrives(see Lecture 20).

    /// <summary>
    /// This class holds all the necessary state to handle a client connection
    /// Note that all of its fields are public because we are using it like a "struct"
    /// It is a simple collection of fields
    /// </summary>
    public class SocketState
    {
        public Socket theSocket;
        public int ID;

        // This is the buffer where we will receive message data from the client
        public byte[] messageBuffer = new byte[1024];

        // This is a larger (growable) buffer, in case a single receive does not contain the full message.
        public StringBuilder sb = new StringBuilder();

        public SocketState(Socket s, int id)
        {
            theSocket = s;
            ID = id;
        }
    }

    public class Networking
    {

        public const int DEFAULT_PORT = 11000;

        // TODO: Move all networking code to this class.
        // Networking code should be completely general-purpose, and useable by any other application.
        // It should contain no references to a specific project.

        /// <summary>
        /// This function should attempt to connect to the server via a provided hostname.
        /// It should save the callback function (in a socket state object) for use when data arrives.
        /// It will need to open a socket and then use the BeginConnect method. 
        /// Note this method takes the "state" object and "regurgitates" it back to you when a connection is made,
        /// thus allowing "communication" between this function and the ConnectedToServer function.
        /// </summary>
        /// <param name="callbackFunction"></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        Socket ConnectToServer(Delegate callbackFunction, hostname string)
        {
            //hostname - the name of the server to connect to

            //callbackFunction - a function inside the View to be called when a connection is made

        }

        /// <summary>
        /// This function is referenced by the BeginConnect method above and is "called" by the OS when the socket connects to the server. 
        /// The "state_in_an_ar_object" object contains a field "AsyncState" which contains the "state" object saved away in the above function.
        /// Once a connection is established the "saved away" callbackFunction needs to called.
        /// This function is saved in the socket state, and was originally passed in to ConnectToServer.
        /// </summary>
        /// <param name="state_in_an_ar_object"></param>
        void ConnectedToServer(IAsyncResult state_in_an_ar_object)
        {


        }

        /// <summary>
        /// The ReceiveCallback method is called by the OS when new data arrives. 
        /// This method should check to see how much data has arrived.
        /// If 0, the connection has been closed (presumably by the server).
        /// On greater than zero data, this method should call the callback function provided above.
        /// </summary>
        /// <param name="state_in_an_ar_object"></param>
        void ReceiveCallback(IAsyncResult state_in_an_ar_object)
        {

        }

        /// <summary>
        /// This is a small helper function that the client View code will call whenever it wants more data.
        /// Note: the client will probably want more data every time it gets data, and has finished processing it in its callbackFunction.
        /// </summary>
        /// <param name=""></param>
        void GetData(state )
        {

        }

        /// <summary>
        /// This function (along with its helper 'SendCallback') will allow a program to send data over a socket. 
        /// This function needs to convert the data into bytes and then send them using socket.BeginSend.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        void Send(Socket socket, String data)
        {

        }

        /// <summary>
        /// This function "assists" the Send function. If all the data has been sent, then life is good and nothing needs to be done 
        /// (note: you may, when first prototyping your program, put a WriteLine in here to see when data goes out).
        /// </summary>
        void SendCallback(IAsyncResult ar)
        {
            Console.WriteLine("SendCallBack: Data has been sent");
            Console.Read();

            SocketState ss = (SocketState)ar.AsyncState;
            // Nothing much to do here, just conclude the send operation so the socket is happy.
            ss.theSocket.EndSend(ar);
        }


    }

}
