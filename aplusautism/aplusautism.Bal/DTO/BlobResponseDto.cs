using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.DTO
{
    public class BlobResponseDto
    {
        public string? Status { get; set; }
        public bool Error { get; set; }
        public BlobDto Blob { get; set; }

        public string fullpath { get; set; }
        public string trialpath { get; set; }
        public string mobilepath { get; set; }

        public BlobResponseDto()
        {
            Blob = new BlobDto();
        }
    }
}
