using System.ComponentModel.DataAnnotations;

namespace HQSoft_EX01.DTOs.Request
{
    public class AuthorRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "The maximum length of {0} is 100 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bio is required.")]

        [MaxLength(500, ErrorMessage = "The maximum length of {0} is 500 characters.")]
        public string Bio { get; set; }
    }
}
