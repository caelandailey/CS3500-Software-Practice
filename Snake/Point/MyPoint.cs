using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SnakeGame
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Point
    {
        public Point(int pointX, int pointY)
        {
            x = pointX;
            y = pointY;
        } 

        [JsonProperty]
        public int x { get; set; }
        [JsonProperty]
        public int y { get; set; }
    }
}
