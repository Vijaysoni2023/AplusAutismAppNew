using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.DTO
{
    public class PostTrailLessonDetailDTO
    {
        public string LanguageId { get; set; }
        public string Title { get; set; }
        public string Manage { get; set; }
        public string Description { get; set; }
        public string Video { get; set; }
        public long LessonSetupId { get; set; }

        public int? LessonCategoryId { get; set; }

        public string traillvideopath { get;set; }    

        public IFormFile trailvideodata { get; set; } 

        public string status { get; set; }

    }
}
