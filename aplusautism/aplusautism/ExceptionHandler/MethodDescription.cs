using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aplusautism.ExceptionHandler
{
    public class MethodDescription : Attribute
    {
        public MethodDescription(string description)
        {
            this.Description = description;
        }
        public string Description { get; set; }
    }
}
