namespace CinemaWorld.Web.Areas.Administration.Views.Genres
{
    using CinemaWorld.Web.Areas.Administration.Views.Shared;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class GenreNavPages : AdminNavPages
    {
        public static string CreateGenre => "CreateGenre";

        public static string GetAll => "GetAll";

        public static string CreateGenreNavClass(ViewContext viewContext) => PageNavClass(viewContext, CreateGenre);

        public static string GetAllNavClass(ViewContext viewContext) => PageNavClass(viewContext, GetAll);
    }
}
