using System;
using System.Collections.Generic;
using System.Linq;

namespace learn
{
    class Hobby
    {
        public int Eid { get; set;}
        public string Hobbyname { get; }
        public string Details { get; }
        
        public Hobby(string hobby, string details)
        {
            Eid=0;
            Hobbyname = hobby;
            Details = details;
        }

        public override string ToString()
        {
            return $"{Hobbyname}: {Details}";
        }
    }
        
}
