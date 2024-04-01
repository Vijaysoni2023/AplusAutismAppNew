using aplusautism.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.DTO
{
    public class LessonDetailsbycategoryDTO
    {
        public int? LessonCategoryId { get;set; }

        public string Categoryname { get; set; }

        public List<LessonSetup> Lessondeatils { get; set; }

        public string LessonNamesValue { get; set; }
        public string UserStatus { get; set; }
    }
    public class DRM_Video_Details
    {
        public string Video_Source_dash { get; set; }
        public string Video_Source_Hls { get; set; }
        public string License_wideVine { get; set; }
        public string License_playReady { get; set; }
        public string License_fairPlay { get; set; }
    }
}
