using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyPoint
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Point
    {
        [JsonProperty]
        private int x;
        [JsonProperty]
        private int y;
    }
}
