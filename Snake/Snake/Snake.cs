using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyPoint;

namespace Snake
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Snake
    {
        [JsonProperty]
        private int ID;

        [JsonProperty]
        private string name;

        [JsonProperty]
        private List<Point> vertices;

        /// <summary>
        /// Determines if this snake collides with the other snake.
        /// </summary>
        public bool CollidesWith(Snake other)
        {
            // If collision
            // Check if head of snake collides with self or another snakes location
            // How do we track a snakes location?
            return true;
        }
    }
}
