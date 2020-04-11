namespace CinemaWorld.Models.ViewModels.News
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Common.Attributes;
    using CinemaWorld.Services.Mapping;

    using Microsoft.AspNetCore.Http;

    using static CinemaWorld.Models.Common.ModelValidation;
    using static CinemaWorld.Models.Common.ModelValidation.News;

    using News = CinemaWorld.Data.Models.News;

    public class NewsEditViewModel : IMapFrom<News>
    {
        public int Id { get; set; }

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

        [DataType(DataType.Upload)]
        [MaxFileSize(ImageMaxSize)]
        [AllowedExtensions]
        [Display(Name = NewImageDisplayName)]
        public IFormFile Image { get; set; }
    }
}
