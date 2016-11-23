/// Caelan Dailey 
/// Karina Biancone
/// 11/22/2016
/// Snake Game
/// CS 3500 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SnakeGame
{

    /// <summary>
    /// Helper class to represent a point. A point has an x or y. Represents a 2d surface
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Point
    {
        /// <summary>
        /// Constructor for point
        /// </summary>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        public Point(int pointX, int pointY)
        {
            x = pointX;
            y = pointY;
        } 

        /// <summary>
        /// The x and y
        /// </summary>
        [JsonProperty]
        public int x { get; set; }
        [JsonProperty]
        public int y { get; set; }
    }
}
