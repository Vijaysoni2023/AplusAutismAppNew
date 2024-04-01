using aplusautism.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.DTO
{
    public class LessonViewDTO
    {
       
        //public List<GlobalCodeCategory> globalCodeCategories { get; set; }
        public List<LessonSetup> Lessonsetup { get; set; }
        public List<GlobalCodes> Language { get; set; }
        public List<GlobalCodes> LessonTitle { get; set; }

        public IFormFile Videodata { get; set; }

        public IFormFile trailvideodata { get; set; }

        public IFormFile mobilevideodata { get; set; }
    }
}
