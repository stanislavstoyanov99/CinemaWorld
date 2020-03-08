namespace CinemaWorld.Web.Areas.Administration.Views.Dashboard
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class MovieNavPages : AdminNavPages
    {
        public static string AddMovie => "AddMovie";

        public static string EditMovie => "EditMovie";

        public static string RemoveMovie => "RemoveMovie";

        public static string GetAll => "GetAll";

        public static string AddMovieNavClass(ViewContext viewContext) => PageNavClass(viewContext, AddMovie);

        public static string RemoveMovieNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditMovie);

        public static string EditMovieNavClass(ViewContext viewContext) => PageNavClass(viewContext, RemoveMovie);

        public static string GetAllNavClass(ViewContext viewContext) => PageNavClass(viewContext, GetAll);
    }
}
