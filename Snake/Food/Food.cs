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
    /// Holds properties that help represent food in the World class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Food
    {
        /// <summary>
        /// Id to track individual food
        /// </summary>
        [JsonProperty]
        public int ID { get; set; }

        /// <summary>
        /// Location to track and draw food
        /// </summary>
        [JsonProperty]
        public Point loc { get; set; }


    }
}
