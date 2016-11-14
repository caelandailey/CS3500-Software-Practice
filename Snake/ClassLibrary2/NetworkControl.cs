using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public Action<SocketState> callMe;
        public SocketState server;

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

        private static int socketID = 0;

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
        public static SocketState ConnectToServer(Action<SocketState> callbackFunction,  string hostName)
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

                SocketState state = new SocketState(socket, socketID);
                socketID++;
                state.theSocket = socket;

                state.callMe = callbackFunction;

                state.theSocket.BeginConnect(ipAddress, Networking.DEFAULT_PORT, ConnectedToServer, state);

                return state;
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

                ProcessMessage(state);
            }

            //state.callMe(state);
            state.theSocket.BeginReceive(state.messageBuffer, 0, state.messageBuffer.Length, SocketFlags.None, ReceiveCallback, state);

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

                Console.WriteLine("received message: \"" + p + "\"");

                byte[] messageBytes = Encoding.UTF8.GetBytes(p);

                // Then remove it from the SocketState's growable buffer
                state.sb.Remove(0, p.Length);
            }
        }

        /// <summary>
        /// This is a small helper function that the client View code will call whenever it wants more data.
        /// Note: the client will probably want more data every time it gets data, and has finished processing it in its callbackFunction.
        /// </summary>
        /// <param name=""></param>
        public static void GetData(SocketState state)
        {
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

            socket.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, socket);
        }

        /// <summary>
        /// This function "assists" the Send function. If all the data has been sent, then life is good and nothing needs to be done 
        /// (note: you may, when first prototyping your program, put a WriteLine in here to see when data goes out).
        /// </summary>
        public static void SendCallback(IAsyncResult state_in_an_ar_object)
        {
            Console.WriteLine("SendCallBack: Data has been sent");
            Console.Read();

            Socket state = (Socket)state_in_an_ar_object.AsyncState;
            // Nothing much to do here, just conclude the send operation so the socket is happy.
            state.EndSend(state_in_an_ar_object);
        }
    }
}
