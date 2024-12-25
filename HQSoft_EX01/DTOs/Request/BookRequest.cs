using HQSoft_EX01.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HQSoft_EX01.Constants;
using HQSoft_EX01.Common.Pagination;

namespace HQSoft_EX01.DTOs.Request
{
    public class BookRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200, ErrorMessage = "The maximum length of {0} is 200 characters.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        [MinValue(0.0)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "AuthorId is required.")]

        public int AuthorId { get; set; }

        public DateTime PublishedDate { get; set; }
    }
    public class SearchBookRequest
    {
        public string SearchKey { get; set; } = "";
        public int? AuthorId { get; set; } = null;
        public DateTime? FromPublishedDate { get; set; } = null;
        public DateTime? ToPublishedDate { get; set; } = null;
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

    }
}
