namespace CinemaWorld.Web.Areas.Administration.Views.Seats
{
    using CinemaWorld.Web.Areas.Administration.Views.Shared;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class SeatsNavPages : AdminNavPages
    {
        public static string CreateSeat => "CreateSeat";

        public static string GetAll => "GetAll";

        public static string CreateSeatsNavClass(ViewContext viewContext) => PageNavClass(viewContext, CreateSeat);

        public static string GetAllNavClass(ViewContext viewContext) => PageNavClass(viewContext, GetAll);
    }
}
