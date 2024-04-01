namespace aplusautism.Models
{
    public class DRM_Response
    {
        public int code { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Data
        {
            public MediaDetails mediaDetails { get; set; }
        }

        public class License
        {
            public string wideVine { get; set; }
            public string playReady { get; set; }
            public string fairPlay { get; set; }
        }

        public class MediaDetails
        {
            public int content_asset_type { get; set; }
            public VideoDetails video_details { get; set; }
            public License license { get; set; }
        }

        

        public class VideoDetails
        {
            public int is_drm { get; set; }
            public string mpeg_path { get; set; }
            public string hls_path { get; set; }
            public string third_party_url { get; set; }
            public string duration { get; set; }
            public double resume_time { get; set; }
            public object subtitle { get; set; }
        }


    }
