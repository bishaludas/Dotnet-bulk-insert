using System;
using System.Collections.Generic;
using System.Linq;

namespace learn
{
    class Hobby
    {
        public string Hobbbyname { get; }
        public string Details { get; }
        
        public Hobby(string hobby, string details)
        {
            Hobbbyname = hobby;
            Details = details;
        }

        public override string ToString()
        {
            return $"{Hobbbyname}: {Details}";
        }
    }
        
}
