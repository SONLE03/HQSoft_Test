using HQSoft_EX01.Common.Pagination;
using HQSoft_EX01.DTOs.Request;
using HQSoft_EX01.DTOs.Response;
using HQSoft_EX01.Models;

namespace HQSoft_EX01.Services.BookService
{
    public interface IBookService
    {
        public Task<PaginatedList<BookResponse>> GetAllBooks(PageInfo pageInfo);
        Task<BookResponse> GetBookById(int bookId);
        Task<BookResponse> CreateBook(BookRequest bookRequest);
        Task<BookResponse> UpdateBook(int bookId, BookRequest bookRequest);
        Task DeleteBook(int bookId);
    }
}
