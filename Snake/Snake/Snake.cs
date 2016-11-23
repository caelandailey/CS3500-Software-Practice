/// Caelan Dailey 
/// Karina Biancone
/// 11/22/2016
/// Snake Game
/// CS 3500 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{

    /// <summary>
    /// Represents our snake. Snake has ID, name, vertices, color, length.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Snake
    {
        /// <summary>
        /// Id to track the snake
        /// </summary>
        [JsonProperty]
        public int ID { get; protected set; }

        /// <summary>
        /// Name of the player snake
        /// </summary>
        [JsonProperty]
        public string name { get; protected set; }

        /// <summary>
        /// Location of the snake
        /// </summary>
        [JsonProperty]
        public List<Point> vertices { get; protected set; }

        /// <summary>
        /// Color to draw
        /// </summary>
        public Color color;

        /// <summary>
        /// track length for score. length = score
        /// </summary>
        public int length;

        /// <summary>
        /// Method to get the length of the snake
        /// </summary>
        /// <returns></returns>
        public int getSnakeLength()
        {
            Point lastPoint = null;
            int snakeLength = 0;

            foreach (Point p in this.vertices) // Loop through points
            {
                if (ReferenceEquals(lastPoint, null)) // if first vertice
                {
                    lastPoint = p;
                    continue;
                }

                snakeLength += Math.Abs(p.x - lastPoint.x); // Get length between 2 points
                snakeLength += Math.Abs(p.y - lastPoint.y);

                lastPoint = p;
            }

            return snakeLength; // Return length of snake
        }
    }
}

