using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Data.Models
{
    public class Cities
    {
        public long Id { get; set; }

        public long? StateId { get; set; }

        public string Name { get; set; }
    }
}
