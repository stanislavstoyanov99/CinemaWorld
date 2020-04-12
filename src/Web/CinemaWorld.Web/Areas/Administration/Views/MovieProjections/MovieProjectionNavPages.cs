namespace CinemaWorld.Web.Areas.Administration.Views.MovieProjections
{
    using CinemaWorld.Web.Areas.Administration.Views.Shared;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class MovieProjectionNavPages : AdminNavPages
    {
        public static string CreateMovieProjection => "CreateMovieProjection";

        public static string GetAll => "GetAll";

        public static string CreateMovieProjectionsNavClass(ViewContext viewContext) => PageNavClass(viewContext, CreateMovieProjection);

        public static string GetAllNavClass(ViewContext viewContext) => PageNavClass(viewContext, GetAll);
    }
}
