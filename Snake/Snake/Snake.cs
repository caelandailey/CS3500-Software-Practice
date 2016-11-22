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
    /// Holds properties that help represent a snake in the World class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Snake
    {
        //the unique integer that represents a single snake
        [JsonProperty]
        public int ID { get; protected set; }

        //the name of the player's snake
        [JsonProperty]
        public string name { get; protected set; }

        //the tail and head's coordinates, along with any points that represent a curve in the snake
        [JsonProperty]
        public List<Point> vertices { get; protected set; }

        //color that the snake will be drawn in
        public Color color;
        

        /// <summary>
        /// Randomly assign a color to the snake
        /// </summary>
        public void setColor()
        {
            Random random = new Random();
            color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        }

        /// <summary>
        /// Returns the color determined for the snake
        /// </summary>
        /// <returns></returns>
        public Color getColor()
        {
            return color;
        }


        /// <summary>
        /// Calculates how long the snake is by adding the length of each segment in the snake.
        /// Will also represent the player's score.
        /// </summary>
        /// <returns></returns>
        public int getSnakeLength()
        {
            Point lastPoint = null;
            int length = 0;

            //cycle through each vertice that the snake has
            foreach (Point p in this.vertices)
            {
                if (ReferenceEquals(lastPoint, null)) // if first vertice
                {
                    length += Math.Abs(p.x - lastPoint.x);
                    length += Math.Abs(p.y - lastPoint.y);
                }

                lastPoint = p;
            }

            return length;
        }

        /// <summary>
        /// Draws only a segment of the snake
        /// </summary>
        /// <param name="e"></param>
        public void drawSegment(PaintEventArgs e)
        {
            using (SolidBrush drawBrush = new SolidBrush(color))
            {
                //Draw lines to screen.
                //e.Graphics.DrawLines(drawBrush, vertices);
            }


        }

    }

}

