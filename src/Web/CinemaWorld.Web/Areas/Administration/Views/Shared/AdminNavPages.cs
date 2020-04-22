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

        public static string Countries => "Countries";

        public static string Contacts => "Contacts";

        public static string About => "About";

        public static string News => "News";

        public static string Halls => "Halls";

        public static string Seats => "Seats";

        public static string MovieProjections => "MovieProjections";

        public static string Privacy => "Privacy";

        public static string MoviesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Movies);

        public static string CinemasNavClass(ViewContext viewContext) => PageNavClass(viewContext, Cinemas);

        public static string GenresNavClass(ViewContext viewContext) => PageNavClass(viewContext, Genres);

        public static string DirectorsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Directors);

        public static string CountriesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Countries);

        public static string ContactsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Contacts);

        public static string AboutNavClass(ViewContext viewContext) => PageNavClass(viewContext, About);

        public static string NewsNavClass(ViewContext viewContext) => PageNavClass(viewContext, News);

        public static string HallNavClass(ViewContext viewContext) => PageNavClass(viewContext, Halls);

        public static string SeatNavClass(ViewContext viewContext) => PageNavClass(viewContext, Seats);

        public static string MovieProjectionsNavClass(ViewContext viewContext) => PageNavClass(viewContext, MovieProjections);

        public static string PrivacyNavClass(ViewContext viewContext) => PageNavClass(viewContext, Privacy);

        protected static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);

            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
