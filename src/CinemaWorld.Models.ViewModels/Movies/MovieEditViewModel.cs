namespace CinemaWorld.Models.ViewModels.Movies
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Common.Attributes;
    using CinemaWorld.Models.ViewModels.Countries;
    using CinemaWorld.Models.ViewModels.Directors;
    using CinemaWorld.Models.ViewModels.Genres;
    using CinemaWorld.Services.Mapping;

    using Microsoft.AspNetCore.Http;

    using static Common.ModelValidation;
    using static Common.ModelValidation.Country;
    using static Common.ModelValidation.Genre;
    using static Common.ModelValidation.Movie;

    using Director = CinemaWorld.Data.Models.Director;
    using Movie = CinemaWorld.Data.Models.Movie;

    public class MovieEditViewModel : IMapFrom<Movie>, IMapFrom<Director>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(
            Common.ModelValidation.Movie.NameMaxLength,
            MinimumLength = Common.ModelValidation.Movie.NameMinLength,
            ErrorMessage = NameLengthError)]
        public string Name { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [DataType(DataType.Date)]
        [Display(Name = DateOfReleaseDisplayName)]
        public DateTime DateOfRelease { get; set; }

        [StringLength(ResolutionMaxLength)]
        public string Resolution { get; set; }

        [Range(1, RatingMaxValue)]
        public decimal Rating { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [DataType(DataType.MultilineText)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionError)]
        public string Description { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(LanguageMaxLength, MinimumLength = LanguageMinLength, ErrorMessage = LanguageError)]
        public string Language { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(CinemaCategoryMaxLength)]
        [Display(Name = CinemaCategoryDisplayName)]
        public string CinemaCategory { get; set; }

        [StringLength(TrailerPathMaxLength, MinimumLength = TrailerPathMinLength, ErrorMessage = TrailerPathError)]
        [DataType(DataType.Url)]
        [Display(Name = TrailerPathDisplayName)]
        public string TrailerPath { get; set; }

        [DataType(DataType.Url)]
        [StringLength(CoverPathMaxLength, MinimumLength = CoverPathMinLength, ErrorMessage = CoverPathError)]
        public string CoverPath { get; set; }

        [DataType(DataType.Upload)]
        [MaxFileSize(CoverImageMaxSize)]
        [AllowedExtensions]
        [Display(Name = NewCoverImageDisplayName)]
        public IFormFile CoverImage { get; set; }

        [DataType(DataType.Url)]
        [StringLength(WallpaperPathMaxLength, MinimumLength = WallpaperPathMinLength, ErrorMessage = WallpaperPathError)]
        public string WallpaperPath { get; set; }

        [DataType(DataType.Upload)]
        [MaxFileSize(WallpaperMaxSize)]
        [AllowedExtensions]
        [Display(Name = NewWallpaperDisplayName)]
        public IFormFile Wallpaper { get; set; }

        [StringLength(ImdbLinkMaxLength, MinimumLength = ImdbLinkMinLength, ErrorMessage = ImdbLinkError)]
        [DataType(DataType.Url)]
        [Display(Name = IMDBLinkDisplayName)]
        public string IMDBLink { get; set; }

        [Range(1, LengthMaxLength)]
        public int Length { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [Display(Name = nameof(Director))]
        public int DirectorId { get; set; }

        public IEnumerable<DirectorDetailsViewModel> Directors { get; set; }

        [Required(ErrorMessage = GenreIdError)]
        [Display(Name = GenresDisplayName)]
        public IList<int> SelectedGenres { get; set; }

        [Required(ErrorMessage = CountryIdError)]
        [Display(Name = CountriesDisplayName)]
        public IList<int> SelectedCountries { get; set; }

        public IEnumerable<GenreDetailsViewModel> Genres { get; set; }

        public IEnumerable<CountryDetailsViewModel> Countries { get; set; }

        public IEnumerable<MovieGenreViewModel> MovieGenres { get; set; }

        public IEnumerable<MovieCountryViewModel> MovieCountries { get; set; }
    }
}
