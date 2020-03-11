namespace CinemaWorld.Web.Areas.Administration.Views.Movies
{
    using CinemaWorld.Web.Areas.Administration.Views.Shared;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class MovieNavPages : AdminNavPages
    {
        public static string CreateMovie => "CreateMovie";

        public static string EditMovie => "EditMovie";

        public static string RemoveMovie => "RemoveMovie";

        public static string GetAll => "GetAll";

        public static string CreateMovieNavClass(ViewContext viewContext) => PageNavClass(viewContext, CreateMovie);

        public static string RemoveMovieNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditMovie);

        public static string EditMovieNavClass(ViewContext viewContext) => PageNavClass(viewContext, RemoveMovie);

        public static string GetAllNavClass(ViewContext viewContext) => PageNavClass(viewContext, GetAll);
    }
}
