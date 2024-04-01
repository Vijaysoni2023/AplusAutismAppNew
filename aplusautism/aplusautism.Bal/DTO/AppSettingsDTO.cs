﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.DTO
{
    public class AppSettingsDTO
    {
        public string? ClientID { get; set; }

        public string? Clientsecret { get; set; }

        public string? SMTPServerName { get; set; }
        public string? SMTPUserName { get; set; }
        public string? SMTPPassword { get; set; }
        public string? SMTPPort { get; set; }
        public string? EmailAccount { get; set; }
        public string? EnableSsl { get; set; }
        public string? ResetpasswordURL { get; set; }


        public string? Secret { get; set; }

        // refresh token time to live (in days), inactive tokens are
        // automatically deleted from the database after this time
        public int RefreshTokenTTL { get; set; }

        public string? Key { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? Subject { get; set; }

        public string? Azure_Path { get; set; }

        public string? BlobConnectionString { get; set; }
        public string? BlobContainerName { get; set; }

        public string? Player_License { get; set; }
        public string? App_ID { get; set; }
        public string? Secret_Key { get; set; }

    }
}
