namespace CinemaWorld.Web.Areas.Administration.Views.Shared
{
    using System;

    using Microsoft.AspNetCore.Mvc.Rendering;

    // TODO : Add other admin pages
    public class AdminNavPages
    {
        public static string Movies => "Movies";

        public static string Cinemas => "Cinemas";

        public static string Genres => "Genres";

        public static string Directors => "Directors";

        public static string MoviesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Movies);

        public static string CinemasNavClass(ViewContext viewContext) => PageNavClass(viewContext, Cinemas);

        public static string GenresNavClass(ViewContext viewContext) => PageNavClass(viewContext, Genres);

        public static string DirectorsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Directors);

        protected static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);

            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
