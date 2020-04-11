namespace CinemaWorld.Models.InputModels.AdministratorInputModels.News
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Common.Attributes;
    using Microsoft.AspNetCore.Http;

    using static CinemaWorld.Models.Common.ModelValidation;
    using static CinemaWorld.Models.Common.ModelValidation.News;

    public class NewsCreateInputModel
    {
        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = TitleLengthError)]
        public string Title { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionLengthError)]
        public string Description { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [Display(Name = ShortDescriptionDisplayName)]
        [StringLength(ShortDescriptionMaxLength, MinimumLength = ShortDescriptionMinLength, ErrorMessage = ShortDescriptionLengthError)]
        public string ShortDescription { get; set; }

        [DataType(DataType.Url)]
        [StringLength(ImagePathMaxLength, MinimumLength = ImagePathMinLength, ErrorMessage = ImagePathLengthError)]
        public string ImagePath { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [DataType(DataType.Upload)]
        [MaxFileSize(ImageMaxSize)]
        [AllowedExtensions]
        public IFormFile Image { get; set; }
    }
}
