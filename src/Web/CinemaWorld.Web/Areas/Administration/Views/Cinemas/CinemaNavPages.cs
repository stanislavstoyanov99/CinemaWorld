namespace CinemaWorld.Web.Areas.Administration.Views.Cinemas
{
    using CinemaWorld.Web.Areas.Administration.Views.Shared;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CinemaNavPages : AdminNavPages
    {
        public static string CreateCinema => "CreateCinema";

        public static string EditCinema => "EditCinema";

        public static string RemoveCinema => "RemoveCinema";

        public static string GetAll => "GetAll";

        public static string CreateCinemaNavClass(ViewContext viewContext) => PageNavClass(viewContext, CreateCinema);

        public static string RemoveCinemaNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditCinema);

        public static string EditCinemaNavClass(ViewContext viewContext) => PageNavClass(viewContext, RemoveCinema);

        public static string GetAllNavClass(ViewContext viewContext) => PageNavClass(viewContext, GetAll);
    }
}
