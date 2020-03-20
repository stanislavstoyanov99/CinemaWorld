namespace CinemaWorld.Web.Areas.Administration.Views.Directors
{
    using CinemaWorld.Web.Areas.Administration.Views.Shared;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class DirectorNavPages : AdminNavPages
    {
        public static string CreateDirector => "CreateDirector";

        public static string GetAll => "GetAll";

        public static string CreateDirectorNavClass(ViewContext viewContext) => PageNavClass(viewContext, CreateDirector);

        public static string GetAllNavClass(ViewContext viewContext) => PageNavClass(viewContext, GetAll);
    }
}
