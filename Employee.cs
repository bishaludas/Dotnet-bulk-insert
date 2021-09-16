using System;
using System.Collections.Generic;
using System.Linq;

namespace learn
{
    class Employee
    {
        public string Name { get; }
        public string Location { get; }
        public List<Hobby> EmpHobby {get;}

        
        public Employee(string name, string location, List<Hobby> empHobby)
        {
            Name = name;
            Location = location;
            EmpHobby = empHobby;

        }

        public override string ToString()
        {
            return $"{Name}: {Location}";
        }
    }
        
}
