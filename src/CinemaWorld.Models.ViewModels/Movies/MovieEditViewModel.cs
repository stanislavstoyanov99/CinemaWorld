namespace CinemaWorld.Models.ViewModels.Movies
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Services.Mapping;

    using static Common.ModelValidation;
    using static Common.ModelValidation.Movie;

    using Movie = CinemaWorld.Data.Models.Movie;

    public class MovieEditViewModel : IMapFrom<Movie>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthError)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = DateOfReleaseDisplayName)]
        public DateTime DateOfRelease { get; set; }

        [StringLength(ResolutionMaxLength)]
        public string Resolution { get; set; }

        [Range(0, RatingMaxValue)]
        public decimal Rating { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionError)]
        public string Description { get; set; }

        [Required]
        [StringLength(LanguageMaxLength, MinimumLength = LanguageMinLength, ErrorMessage = LanguageError)]
        public string Language { get; set; }

        [Required]
        [StringLength(CinemaCategoryMaxLength)]
        [Display(Name = CinemaCategoryDisplayName)]
        public string CinemaCategory { get; set; }

        [StringLength(TrailerPathMaxLength, MinimumLength = TrailerPathMinLength, ErrorMessage = TrailerPathError)]
        [Display(Name = TrailerPathDisplayName)]
        public string TrailerPath { get; set; }

        [Required]
        [StringLength(CoverPathMaxLength, MinimumLength = CoverPathMinLength, ErrorMessage = CoverPathError)]
        [Display(Name = CoverPathDisplayName)]
        public string CoverPath { get; set; }

        [StringLength(ImdbLinkMaxLength, MinimumLength = ImdbLinkMinLength, ErrorMessage = ImdbLinkError)]
        [Display(Name = IMDBLinkDisplayName)]
        public string IMDBLink { get; set; }

        [Range(1, LengthMaxLength)]
        public int Length { get; set; }

        [Required]
        public int DirectorId { get; set; }

        public string DirectorName { get; set; }
    }
}
