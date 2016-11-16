using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyPoint;

namespace Snake
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Food
    {
        [JsonProperty]
        private int ID;
        
        [JsonProperty]
        private Point loc;


    }
}
