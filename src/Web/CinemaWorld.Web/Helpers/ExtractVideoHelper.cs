namespace CinemaWorld.Web.Helpers
{
    using System;
    using System.Linq;
    using System.Web;

    public static class ExtractVideoHelper
    {
        public static string ExtractVideoId(string trailerPath)
        {
            var uri = new Uri(trailerPath);
            var query = HttpUtility.ParseQueryString(uri.Query);

            var videoId = string.Empty;

            if (query.AllKeys.Contains("v"))
            {
                videoId = query["v"];
            }
            else
            {
                videoId = uri.Segments.Last();
            }

            return videoId;
        }
    }
}
