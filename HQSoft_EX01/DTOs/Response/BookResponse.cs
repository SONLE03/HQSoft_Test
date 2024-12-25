using HQSoft_EX01.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HQSoft_EX01.DTOs.Response
{
    public class BooksWithAuthors
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public DateTime PublishedDate { get; set; }
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
    }
    public class BookResponse
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public DateTime PublishedDate { get; set; }
        public AuthorResponse AuthorResponse { get; set; }
    }

}
