namespace CinemaWorld.Web.Areas.Administration.Views.Halls
{
    using CinemaWorld.Web.Areas.Administration.Views.Shared;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class HallNavPages : AdminNavPages
    {
        public static string CreateHall => "CreateHall";

        public static string GetAll => "GetAll";

        public static string CreateHallNavClass(ViewContext viewContext) => PageNavClass(viewContext, CreateHall);

        public static string GetAllNavClass(ViewContext viewContext) => PageNavClass(viewContext, GetAll);
    }
}
