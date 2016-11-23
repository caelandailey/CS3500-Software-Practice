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

        public int length;

        public void setColor(Color _color)
        {
            color = _color;
        }

        public Color getColor()
        {
            return color;
        }


        public int getSnakeLength()
        {
            Point lastPoint = null;
            int snakeLength = 0;

            foreach (Point p in this.vertices)
            {
                if (ReferenceEquals(lastPoint, null)) // if first vertice
                {
                    lastPoint = p;
                    continue;

                }
                snakeLength += Math.Abs(p.x - lastPoint.x);
                snakeLength += Math.Abs(p.y - lastPoint.y);

                lastPoint = p;
            }

            return snakeLength;
        }
    }
}

