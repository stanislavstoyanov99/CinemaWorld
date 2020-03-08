namespace CinemaWorld.Web.Areas.Administration.Views.Dashboard
{
    using System;

    using Microsoft.AspNetCore.Mvc.Rendering;

    // TODO : Add other admin pages
    public class AdminNavPages
    {
        public static string Movies => "Movies";

        public static string Cinemas => "Cinemas";

        public static string MoviesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Movies);

        public static string CinemasNavClass(ViewContext viewContext) => PageNavClass(viewContext, Cinemas);

        protected static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);

            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
