namespace CinemaWorld.Data.Common
{
    public static class DataValidation
    {
        public const int NameMaxLength = 30;
        public const int FullNameMaxLength = 60;

        public static class Cinema
        {
            public const int NameMaxLength = 60;
            public const int AddressMaxLength = 500;
        }

        public static class Country
        {
            public const int NameMaxLength = 40;
        }

        public static class Genre
        {
            public const int NameMaxLength = 30;
        }

        public static class Movie
        {
            public const int NameMaxLength = 60;
            public const int DescriptionMaxLength = 1100;
            public const int LanguageMaxLength = 50;
            public const int TrailerPathMaxLength = 500;
            public const int ImdbLinkMaxLength = 500;
            public const int CoverPathMaxLength = 500;
        }

        public static class News
        {
            public const int TitleMaxLength = 100;
            public const int DescriptionMaxLength = 1200;
        }

        public static class Promotion
        {
            public const int DescriptionMaxLength = 1000;
        }

        public static class Review
        {
            public const int TitleMaxLength = 100;
            public const int DescriptionMaxLength = 1500;
        }
    }
}
