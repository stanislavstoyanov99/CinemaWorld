namespace CinemaWorld.Models.InputModels.AdministratorInputModels.Contacts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Models.ViewModels.Contacts;

    using static CinemaWorld.Models.Common.ModelValidation.AdminContactFormEntry;

    public class SendContactInputModel
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(FullNameMaxLegth, MinimumLength = FullNameMinLength, ErrorMessage = FullNameLengthError)]
        [Display(Name = FullNameDisplayName)]
        public string FullName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(SubjectMaxLength, MinimumLength = SubjectMinLegth, ErrorMessage = SubjectLengthError)]
        public string Subject { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLegth, ErrorMessage = ContentLengthError)]
        public string Content { get; set; }

        public IEnumerable<UserEmailViewModel> UserEmails { get; set; }
    }
}
