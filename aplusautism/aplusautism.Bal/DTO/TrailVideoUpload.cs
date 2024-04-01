using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.DTO
{
    public class TrailVideoUpload : PostTrailLessonDetailDTO
    { 
        public IFormFile videodata { get; set; }
    }
}
