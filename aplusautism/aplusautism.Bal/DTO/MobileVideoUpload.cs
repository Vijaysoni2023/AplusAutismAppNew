using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.DTO
{
    public class MobileVideoUpload : PostMobileLessonDetailDTO
    { 
        public IFormFile mobilevideodata { get; set; }
    }
}
