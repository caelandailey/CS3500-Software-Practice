/// Caelan Dailey 
/// Karina Biancone
/// 11/22/2016
/// Snake Game
/// CS 3500 


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
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
        public Socket theSocket; // Holds information
        public int ID; // Id of socket to keep track
        public Action<SocketState> callMe; // Action calls back in order to communicate. Takes in a method and calls 'me'


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

    /// <summary>
    /// This class holds the necesarry information that the networks needs from the server, a TCP listener and  
    /// delgate.
    /// </summary>
    public class ServerState
    {
        public TcpListener listener;
        public AsyncCallback callMe;
    }

    /// <summary>
    /// The communicator between the Server and Client.
    /// </summary>
    public class Networking
    {

        public const int DEFAULT_PORT = 11000;

        private static int socketID = 0;

        public static SocketState server;

        private static int clientCount = 0;
        private static object clientLock = new object();

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
        public static SocketState ConnectToServer(Action<SocketState> callbackFunction, string hostName)
        {
            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo;
                IPAddress ipAddress = IPAddress.None;

                // Determine if the server address is a URL or an IP
                try
                {
                    ipHostInfo = Dns.GetHostEntry(hostName);
                    bool foundIPV4 = false;
                    foreach (IPAddress addr in ipHostInfo.AddressList)
                        if (addr.AddressFamily != AddressFamily.InterNetworkV6)
                        {
                            foundIPV4 = true;
                            ipAddress = addr;
                            break;
                        }
                    // Didn't find any IPV4 addresses
                    if (!foundIPV4)
                    {
                        System.Diagnostics.Debug.WriteLine("Invalid addres: " + hostName);
                        return null;
                    }
                }
                catch (Exception e1)
                {
                    // see if host name is actually an ipaddress, i.e., 155.99.123.456
                    System.Diagnostics.Debug.WriteLine("using IP");
                    ipAddress = IPAddress.Parse(hostName);
                }

                Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                server = new SocketState(socket, socketID);

                socketID++;
                server.theSocket = socket;

                server.callMe = callbackFunction;

                server.theSocket.BeginConnect(ipAddress, Networking.DEFAULT_PORT, ConnectedToServer, server);

                return server;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return null;
            }
        }

        /// <summary>
        /// This function is referenced by the BeginConnect method above and is "called" by the OS when the socket connects to the server. 
        /// The "state_in_an_ar_object" object contains a field "AsyncState" which contains the "state" object saved away in the above function.
        /// Once a connection is established the "saved away" callbackFunction needs to called.
        /// This function is saved in the socket state, and was originally passed in to ConnectToServer.
        /// </summary>
        /// <param name="state_in_an_ar_object"></param>
        public static void ConnectedToServer(IAsyncResult state_in_an_ar_object)
        {
            SocketState state = (SocketState)state_in_an_ar_object.AsyncState;

            state.theSocket.EndConnect(state_in_an_ar_object);

            state.callMe(state);

            state.theSocket.BeginReceive(state.messageBuffer, 0, state.messageBuffer.Length, SocketFlags.None, ReceiveCallback, state);
        }

        /// <summary>
        /// The ReceiveCallback method is called by the OS when new data arrives. 
        /// This method should check to see how much data has arrived.
        /// If 0, the connection has been closed (presumably by the server).
        /// On greater than zero data, this method should call the callback function provided above.
        /// </summary>
        /// <param name="state_in_an_ar_object"></param>
        public static void ReceiveCallback(IAsyncResult state_in_an_ar_object)
        {
            SocketState state = (SocketState)state_in_an_ar_object.AsyncState;

            //state.theSocket.EndReceive(state_in_an_ar_object);

            //(append message to state)

            int bytesRead = state.theSocket.EndReceive(state_in_an_ar_object);

            // If the socket is still open
            if (bytesRead > 0)
            {
                string theMessage = Encoding.UTF8.GetString(state.messageBuffer, 0, bytesRead);
                // Append the received data to the growable buffer.
                // It may be an incomplete message, so we need to start building it up piece by piece
                state.sb.Append(theMessage);
                

                //ProcessMessage(state);
                state.callMe(state);
            }


            //state.theSocket.BeginReceive(state.messageBuffer, 0, state.messageBuffer.Length, SocketFlags.None, ReceiveCallback, state);

        }

        /// <summary>
        /// This is a small helper function that the client View code will call whenever it wants more data.
        /// Note: the client will probably want more data every time it gets data, and has finished processing it in its callbackFunction.
        /// </summary>
        /// <param name=""></param>
        public static void GetData(SocketState state)
        {
            //state.callMe(state);
            state.theSocket.BeginReceive(state.messageBuffer, 0, state.messageBuffer.Length, SocketFlags.None, ReceiveCallback, state);
        }

        /// <summary>
        /// This function (along with its helper 'SendCallback') will allow a program to send data over a socket. 
        /// This function needs to convert the data into bytes and then send them using socket.BeginSend.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        public static void Send(Socket socket, String data)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(data + "\n");
            socket.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, server);
        }

        /// <summary>
        /// This function "assists" the Send function. If all the data has been sent, then life is good and nothing needs to be done 
        /// (note: you may, when first prototyping your program, put a WriteLine in here to see when data goes out).
        /// </summary>
        public static void SendCallback(IAsyncResult state_in_an_ar_object)
        {
            SocketState state = (SocketState)state_in_an_ar_object.AsyncState;
            // Nothing much to do here, just conclude the send operation so the socket is happy.
            state.theSocket.EndSend(state_in_an_ar_object);
        }

        /// <summary>
        /// Stores a delegate and listener in a state to then begin accepting a connection
        /// </summary>
        /// <param name="callBack"></param>
        public static void ServerAwaitingClientLoop(Action<SocketState> callBack)
        {
            //the new state to hold delegate and listener
            ServerState state = new ServerState();
            
            state.listener = new TcpListener(IPAddress.Any, 11000);
            state.callMe = callBack;
            state.listener.Start();
            //begin accepting socket which will call on AcceptNewClient
            state.listener.BeginAcceptSocket(AcceptNewClient, state);
        }

        /// <summary>
        /// Will add a new client to the server and begin the event loop.
        /// </summary>
        /// <param name="ar"></param>
        static void AcceptNewClient(IAsyncResult ar)
        {
            ServerState state = (ServerState)ar.AsyncState;
            Socket socket = state.listener.EndAcceptSocket(ar);
            lock (Networking.clientLock)
            {
                Networking.clientCount++;
                SocketState socketState = new SocketState(socket, Networking.clientCount);

                socketState.theSocket = socket; // ?
                //socketState.callMe = state.callMe;

                //state.callMe(socketState);
                socketState.callMe(socketState);
                
            }            

            state.listener.BeginAcceptSocket(AcceptNewClient, state);
        }


    }
}
