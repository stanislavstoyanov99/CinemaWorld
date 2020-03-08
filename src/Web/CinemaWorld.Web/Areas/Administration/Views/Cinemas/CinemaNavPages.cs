namespace CinemaWorld.Web.Areas.Administration.Views.Dashboard
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CinemaNavPages : AdminNavPages
    {
        public static string AddCinema => "AddMovie";

        public static string EditCinema => "EditMovie";

        public static string RemoveCinema => "RemoveMovie";

        public static string GetAll => "GetAll";

        public static string AddCinemaNavClass(ViewContext viewContext) => PageNavClass(viewContext, AddCinema);

        public static string RemoveCinemaNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditCinema);

        public static string EditCinemaNavClass(ViewContext viewContext) => PageNavClass(viewContext, RemoveCinema);

        public static string GetAllNavClass(ViewContext viewContext) => PageNavClass(viewContext, GetAll);
    }
}
