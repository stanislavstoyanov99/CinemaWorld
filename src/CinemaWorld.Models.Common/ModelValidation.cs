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

            public const int ImdbLinkMinLength = 10;
            public const int ImdbLinkMaxLength = 500;

            public const int LengthMaxLength = 300;

            public const string DescriptionError = "Description must be between {2} and {1} symbols";
            public const string LanguageError = "Language must be between {2} and {1} symbols";
            public const string TrailerPathError = "Trailer path must be between {2} and {1} symbols";
            public const string CoverPathError = "Cover path must be between {2} and {1} symbols";
            public const string ImdbLinkError = "IMDB link must be between {2} and {1} symbols";

            public const string DateOfReleaseDisplayName = "Date of release";
            public const string CinemaCategoryDisplayName = "Cinema Category";
            public const string TrailerPathDisplayName = "Trailer Path";
            public const string CoverPathDisplayName = "Cover Path";
            public const string IMDBLinkDisplayName = "IMDB Link";

            public const string IdDisplayName = "No.";
            public const string NameDisplayName = "Movie Name";
            public const string DateOfReleaseAllMoviesDisplayName = "Year";
            public const string OnlyDateAllMoviesDisplayName = "Date of release";

            public const string MovieGenreDisplayName = "Genre Name";
            public const string MovieCountryDisplayName = "Country Name";
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
    }
}
