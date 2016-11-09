using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkController
{
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
    }

}
