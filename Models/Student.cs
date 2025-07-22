using System.ComponentModel.DataAnnotations;

namespace StudentBackend.Models
{
    public class Student
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string IDNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", 
            ErrorMessage = "Email must be a valid format with a proper domain (e.g., user@example.com)")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime EnrollmentDate { get; set; }
    }
} 