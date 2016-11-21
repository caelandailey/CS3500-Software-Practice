using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Drawing;

namespace SnakeGame
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Snake
    {
        [JsonProperty]
        public int ID { get; protected set; }

        [JsonProperty]
        public string name { get; protected set; }

        [JsonProperty]
        public List<Point> vertices { get; protected set; }

        public Color color;

        public void setColor()
        {
            //read id and set it to a random color
            Random random = new Random();
            color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        }

        public Color getColor()
        {
            return color;
        }


        public int getSnakeLength()
        {
            Point lastPoint = null;
            int length = 0;

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


        public void drawSegment(PaintEventArgs e)
        {
            using (SolidBrush drawBrush = new SolidBrush(color))
            {
                //Draw lines to screen.
                e.Graphics.DrawLines(drawBrush, vertices);
            }


        }

    }

}
}
