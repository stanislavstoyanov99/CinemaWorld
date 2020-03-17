namespace CinemaWorld.Models.ViewModels.Movies
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Models.ViewModels.Directors;
    using CinemaWorld.Models.ViewModels.Genres;
    using CinemaWorld.Services.Mapping;

    using static Common.ModelValidation;
    using static Common.ModelValidation.Movie;

    using Director = CinemaWorld.Data.Models.Director;
    using Movie = CinemaWorld.Data.Models.Movie;

    public class MovieEditViewModel : IMapFrom<Movie>, IMapFrom<Director>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthError)]
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

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [DataType(DataType.Url)]
        [StringLength(CoverPathMaxLength, MinimumLength = CoverPathMinLength, ErrorMessage = CoverPathError)]
        [Display(Name = CoverPathDisplayName)]
        public string CoverPath { get; set; }

        [StringLength(ImdbLinkMaxLength, MinimumLength = ImdbLinkMinLength, ErrorMessage = ImdbLinkError)]
        [DataType(DataType.Url)]
        [Display(Name = IMDBLinkDisplayName)]
        public string IMDBLink { get; set; }

        [Range(1, LengthMaxLength)]
        public int Length { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [Display(Name = nameof(Director))]
        public int DirectorId { get; set; }

        public IEnumerable<DirectorViewModel> Directors { get; set; }

        public IList<int> SelectedGenres { get; set; }

        public IEnumerable<GenreDetailsViewModel> Genres { get; set; }
    }
}
