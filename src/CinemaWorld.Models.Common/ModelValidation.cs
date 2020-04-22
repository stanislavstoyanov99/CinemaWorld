namespace CinemaWorld.Models.Common
{
    public static class ModelValidation
    {
        public const string NameLengthError = "Name must be between {2} and {1} symbols";
        public const string EmptyFieldLengthError = "Please enter the field.";

        public static class Movie
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 60;

            public const int ResolutionMaxLength = 2;

            public const int RatingMaxValue = 10;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 1100;

            public const int LanguageMinLength = 3;
            public const int LanguageMaxLength = 50;

            public const int CinemaCategoryMaxLength = 1;

            public const int TrailerPathMinLength = 10;
            public const int TrailerPathMaxLength = 500;

            public const int CoverPathMinLength = 10;
            public const int CoverPathMaxLength = 500;

            public const int WallpaperPathMinLength = 10;
            public const int WallpaperPathMaxLength = 500;

            public const int ImdbLinkMinLength = 10;
            public const int ImdbLinkMaxLength = 500;

            public const int LengthMaxLength = 300;

            public const string DescriptionError = "Description must be between {2} and {1} symbols";
            public const string LanguageError = "Language must be between {2} and {1} symbols";
            public const string TrailerPathError = "Trailer path must be between {2} and {1} symbols";
            public const string CoverPathError = "Cover path must be between {2} and {1} symbols";
            public const string WallpaperPathError = "Wallpaper must be between {2} and {1} symbols";
            public const string ImdbLinkError = "IMDB link must be between {2} and {1} symbols";

            public const string DateOfReleaseDisplayName = "Date of release";
            public const string CinemaCategoryDisplayName = "Cinema Category";
            public const string TrailerPathDisplayName = "Trailer Path";
            public const string NewCoverImageDisplayName = "New Cover Image";
            public const string CoverImageDisplayName = "Cover Image";
            public const string NewWallpaperDisplayName = "New Wallpaper";
            public const string WallpaperDisplayName = "Wallpaper";
            public const string IMDBLinkDisplayName = "IMDB Link";

            public const string IdDisplayName = "No.";
            public const string NameDisplayName = "Movie Name";
            public const string DateOfReleaseAllMoviesDisplayName = "Year";
            public const string OnlyDateAllMoviesDisplayName = "Date of release";

            public const int CoverImageMaxSize = 10 * 1024 * 1024;
            public const int WallpaperMaxSize = 15 * 1024 * 1024;

            public const string MovieIdError = "Please select movie.";
        }

        public static class Country
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 40;
            public const string CountriesDisplayName = "Countries";
            public const string CountryIdError = "Please select country name.";
        }

        public static class Director
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;
            public const string FullNameDisplayName = "Director Full Name";
            public const string FirstNameDisplayName = "First Name";
            public const string LastNameDisplayName = "Last Name";
            public const string DirectorIdError = "Please select director name.";
        }

        public static class Genre
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;
            public const string NameDisplayName = "Genre Name";
            public const string GenresDisplayName = "Genres";
            public const string GenreIdError = "Please select genre name.";
        }

        public static class Cinema
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 60;
            public const int AddressMaxLength = 500;
            public const int AddressMinLength = 20;

            public const string AddressLengthError = "Address must be between {2} and {1} symbols";
            public const string NameDisplayName = "Cinema Name";

            public const string CinemaIdError = "Please select cinema.";
        }

        public static class ContactFormEntry
        {
            public const int FirstNameMinLength = 3;
            public const int FirstNameMaxLength = 30;
            public const int LastNameMinLength = 3;
            public const int LastNameMaxLength = 30;

            public const int SubjectMaxLength = 100;
            public const int SubjectMinLegth = 5;

            public const int ContentMaxLength = 10000;
            public const int ContentMinLegth = 20;

            public const string FirstNameLengthError = "First name must be between {2} and {1} symbols";
            public const string LastNameLengthError = "Last name must be between {2} and {1} symbols";
            public const string SubjectLengthError = "Subject must be between {2} and {1} symbols";
            public const string ContentLengthError = "Content must be between {2} and {1} symbols";

            public const string FirstNameDisplayName = "First Name";
            public const string LastNameDispalyName = "Last Name";
        }

        public static class AdminContactFormEntry
        {
            public const int FullNameMinLength = 10;
            public const int FullNameMaxLegth = 60;

            public const int SubjectMaxLength = 100;
            public const int SubjectMinLegth = 5;

            public const int ContentMaxLength = 10000;
            public const int ContentMinLegth = 20;

            public const string FullNameLengthError = "Full name must be between {2} and {1} symbols";
            public const string SubjectLengthError = "Subject must be between {2} and {1} symbols";
            public const string ContentLengthError = "Content must be between {2} and {1} symbols";

            public const string FullNameDisplayName = "Full Name";
        }

        public static class FaqEntry
        {
            public const int QuestionMinLength = 10;
            public const int QuestionMaxLength = 100;

            public const int AnswerMinLength = 10;
            public const int AnswerMaxLength = 1000;

            public const string QuestionLengthError = "Question must be between {2} and {1} symbols";
            public const string AnswerLengthError = "Answer must be between {2} and {1} symbols";
        }

        public static class News
        {
            public const int TitleMinLength = 10;
            public const int TitleMaxLength = 100;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 10000;

            public const int ShortDescriptionMinLength = 10;
            public const int ShortDescriptionMaxLength = 400;

            public const int ImagePathMinLength = 10;
            public const int ImagePathMaxLength = 500;

            public const string TitleLengthError = "Title must be between {2} and {1} symbols";
            public const string DescriptionLengthError = "Description must be between {2} and {1} symbols";
            public const string ShortDescriptionLengthError = "Short description must be between {2} and {1} symbols";
            public const string ImagePathLengthError = "Image path must be between {2} and {1} symbols";

            public const string NewImageDisplayName = "New Image";
            public const string ImagePathDisplayName = "Image Path";
            public const int ImageMaxSize = 10 * 1024 * 1024;

            public const string CreationDateDisplayName = "Creation Date";
            public const string ShortDescriptionDisplayName = "Short Description";
        }

        public static class Hall
        {
            public const int CapacityMinLength = 1;
            public const int CapacityMaxLength = 150;

            public const string HallIdError = "Please select hall id.";
        }

        public static class Seat
        {
            public const string IsAvailableDisplayName = "Is Available";
            public const string IsSoldDisplayName = "Is Sold";

            public const string RowNumberDisplayName = "Row Number";
            public const string NumberDisplayName = "Seat Number";
            public const string HallIdDisplayName = "Hall Id";

            public const string RowNumberIdError = "Please select row number.";
            public const string SeatNumberIdError = "Please select seat number.";
        }

        public static class Ticket
        {
            public const string InvalidSeat = "Please choose seat";
            public const string MissingTicketType = "Please choose ticket type";
        }

        public static class Privacy
        {
            public const int PageContentMinLength = 1000;
            public const int PageContentMaxLength = 15000;

            public const string PageContentLengthError = "Page content must be between {2} and {1} symbols";
            public const string PageContentDisplayName = "Page Content";
        }
    }
}
