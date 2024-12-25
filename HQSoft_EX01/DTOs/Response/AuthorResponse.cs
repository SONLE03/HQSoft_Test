using System.ComponentModel.DataAnnotations;

namespace HQSoft_EX01.DTOs.Response
{
    public class AuthorResponse
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
    }
}
