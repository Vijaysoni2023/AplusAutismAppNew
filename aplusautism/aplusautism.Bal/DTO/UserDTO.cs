﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.DTO
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }

        public int? count { get; set; }
    }

    public class ForgetPasswordDTO
    {
        public string FirstName { get; set; }
        public string Password { get; set; }
    }
}
