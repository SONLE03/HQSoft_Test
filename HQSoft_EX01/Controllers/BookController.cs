using HQSoft_EX01.Common.Pagination;
using HQSoft_EX01.Common;
using HQSoft_EX01.Constants;
using HQSoft_EX01.DTOs.Request;
using HQSoft_EX01.Services.AuthorService;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using HQSoft_EX01.Services.BookService;

namespace HQSoft_EX01.Controllers
{
    [ApiController]
    [Route(Routes.BOOKS)]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpGet("fetch")]
        public async Task<IActionResult> GetAllBooks([FromQuery] PageInfo pageInfo)
        {
            return new SuccessfulResponse<object>(await _bookService.GetAllBooks(pageInfo), (int)HttpStatusCode.OK, "Get books successfully").GetResponse();
        }
        [HttpGet("fetch/{bookId}")]
        public async Task<IActionResult> GetBookById(int bookId)
        {
            return new SuccessfulResponse<object>(await _bookService.GetBookById(bookId), (int)HttpStatusCode.OK, "Get book successfully").GetResponse();
        }
        [HttpPost("insert")]
        public async Task<IActionResult> CreateBook(BookRequest bookRequest)
        {
            return new SuccessfulResponse<object>(await _bookService.CreateBook(bookRequest), (int)HttpStatusCode.OK, "Your book has been successfully added").GetResponse();
        }
        [HttpPut("update/{bookId}")]
        public async Task<IActionResult> UpdateBook(int bookId, BookRequest bookRequest)
        {
            return new SuccessfulResponse<object>(await _bookService.UpdateBook(bookId, bookRequest), (int)HttpStatusCode.OK, "Your book has been successfully mpdified").GetResponse();
        }
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            await _bookService.DeleteBook(bookId);
            return new SuccessfulResponse<object>(null, (int)HttpStatusCode.OK, "Your book has been successfully deleted").GetResponse();
        }
    }
}
