using AutoMapper;
using HQSoft_EX01.Common.Pagination;
using HQSoft_EX01.Data;
using HQSoft_EX01.DTOs.Request;
using HQSoft_EX01.DTOs.Response;
using HQSoft_EX01.Exceptions;
using HQSoft_EX01.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HQSoft_EX01.Services.BookService
{
    public class BookServiceImp : IBookService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        public BookServiceImp(ApplicationDBContext context, IMapper map)
        {
            _context = context;
            _mapper = map;
        }
        public async Task<BookResponse> CreateBook(BookRequest bookRequest)
        {
            var author = await _context.Authors.FindAsync(bookRequest.AuthorId);
            if (author == null)
            {
                throw new ObjectNotFoundException("Author not found");
            }
            var book = new Books
            {
                Title = bookRequest.Title,
                Price = bookRequest.Price,
                Author = author,
                PublishedDate = bookRequest.PublishedDate
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return _mapper.Map<BookResponse>(book);
        }

        public async Task DeleteBook(int bookId)
        {
            // Kiểm tra theo Id dùng @Param để xóa bản ghi
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ObjectNotFoundException("Book not found");
            }
        }

        public async Task<PaginatedList<BookResponse>> GetAllBooks(PageInfo pageInfo)
        {
            // Gọi stored procedure và lấy dữ liệu
            var booksWithAuthors = await _context.Set<BooksWithAuthors>()
                .FromSqlRaw("EXEC GetAllBooksPaged @PageNumber = {0}, @PageSize = {1}", pageInfo.PageNumber, pageInfo.PageSize)
                .ToListAsync();
            var bookResponse = _mapper.Map<List<BookResponse>>(booksWithAuthors);
            var totalCount = await _context.Books.CountAsync(); // Lấy tổng số tác giả trong bảng
            var paginatedList = new PaginatedList<BookResponse>(bookResponse, totalCount, pageInfo.PageNumber, pageInfo.PageSize);

            return paginatedList;
        }




        public async Task<BookResponse> GetBookById(int bookId)
        {
            // Gọi stored procedure GetAuthorById
            var books = await _context.Set<BooksWithAuthors>()
                .FromSqlRaw("EXEC GetBookById @BookId",
                            new SqlParameter("@BookId", bookId))
               .ToListAsync();
            var book = books.FirstOrDefault();
            if (book == null)
            {
                throw new ObjectNotFoundException("Book not found");
            }
            return _mapper.Map<BookResponse>(book);
        }

        public async Task<BookResponse> UpdateBook(int bookId, BookRequest bookRequest)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                throw new ObjectNotFoundException("Book not found");
                
            }
            var author = await _context.Authors.FindAsync(bookRequest.AuthorId);
            if (author == null)
            {
                throw new ObjectNotFoundException("Author not found");
            }
            book.Title = bookRequest.Title;
            book.Price = bookRequest.Price;
            book.Author = author;
            book.PublishedDate = bookRequest.PublishedDate;
            await _context.SaveChangesAsync();
            return _mapper.Map<BookResponse>(book);
        }
    }
}
