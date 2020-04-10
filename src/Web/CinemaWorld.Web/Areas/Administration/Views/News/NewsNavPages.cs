namespace CinemaWorld.Web.Areas.Administration.Views.News
{
    using CinemaWorld.Web.Areas.Administration.Views.Shared;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class NewsNavPages : AdminNavPages
    {
        public static string CreateNews => "CreateNews";

        public static string GetAll => "GetAll";

        public static string CreateNewsNavClass(ViewContext viewContext) => PageNavClass(viewContext, CreateNews);

        public static string GetAllNavClass(ViewContext viewContext) => PageNavClass(viewContext, GetAll);
    }
}
