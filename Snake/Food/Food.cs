using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SnakeGame
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Food
    {
        [JsonProperty]
        public int ID { get; protected set; }

        [JsonProperty]
        public Point loc { get; protected set; }


    }
}
