using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess3
{

    public struct Player // Player is a struct it has no methods or constructor. I have put in IP address in a future release there may be a web function.
        // I am very interested in what ports I should use to send board data packets and how to convert them into bytes or text or xml or what.
    {
        public string name { get; set; }
        public string ipAddress;
        public wB colour { get; set; }
        public huAi huai { get; set; }
        public handicap handicap { get; set; }
        
    }


}
