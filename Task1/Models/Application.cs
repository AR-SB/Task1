using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1.Models
{
    [Table("Applications")]
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }
       
        public int? EmployeeId { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid E-mail address.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        [Display(Name = "Telephone Number")]
        public string TelephoneNumber { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }
        [NotMapped]
        public IFormFile PdfFile { get; set; }
        public string FileName { get; set; }
        public GenderType Gender { get; set; }

        public string Major { get; set; }

        public bool? ApplicationStatus { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public DateTime SubmissionTimestamp { get; set; }

        [Display(Name = "Action Date")]
        [DataType(DataType.DateTime)]
        public DateTime? ActionDate { get; set; }


    }

    public enum GenderType
    {
        Male,
        Female,
        Other
    }
}
