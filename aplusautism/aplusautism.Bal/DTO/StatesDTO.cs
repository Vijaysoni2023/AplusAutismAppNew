﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.DTO
{
    public class StatesDTO
    {
        public long Id { get; set; }

        public long? CountryId { get; set; }

        public long? StateId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}
