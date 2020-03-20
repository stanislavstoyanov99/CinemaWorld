namespace CinemaWorld.Services.Data.Common
{
    public static class ExceptionMessages
    {
        public const string MovieNotFound = "Movie with id {0} is not found.";

        public const string MovieAlreadyExists = "Movie with id {0} already exists";

        public const string DirectorNotFound = "Director with id {0} is not found.";

        public const string InvalidCinemaCategoryType = "Cinema category type {0} is invalid.";

        public const string GenreNotFound = "Genre with id {0} is not found.";

        public const string GenreAlreadyExists = "Genre with id {0} already exists";

        public const string MovieGenreNotFound = "Movie's genre with movie id {0} and genre id {1} is not found.";

        public const string DirectorAlreadyExists = "Director with id {0} already exists";
    }
}
