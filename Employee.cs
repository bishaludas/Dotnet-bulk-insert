using System;
using System.Collections.Generic;
using System.Linq;

namespace learn
{
    class Employee
    {
        public string Name { get; }
        public string Location { get; }
        
        public Employee(string name, string location)
        {
            Name = name;
            Location = location;
        }

        public override string ToString()
        {
            return $"{Name}: {Location}";
        }
    }
        
}
