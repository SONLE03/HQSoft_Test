using AutoMapper;
using Azure;
using HQSoft_EX01.Common.Pagination;
using HQSoft_EX01.Data;
using HQSoft_EX01.DTOs.Request;
using HQSoft_EX01.DTOs.Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using HQSoft_EX01.Exceptions;

namespace HQSoft_EX01.Services.ReportService
{
    public class ReportServiceImp : IReportService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        public ReportServiceImp(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<BookResponse>> GetReportsBook(SearchBookRequest searchBookRequest)
        {
            var fromPublishedDate = searchBookRequest.FromPublishedDate;
            var toPublishedDate = searchBookRequest.ToPublishedDate;
            if (fromPublishedDate != null && toPublishedDate != null && fromPublishedDate > toPublishedDate)
            {
                throw new BusinessException("fromPublishedDate cannot be greater than the toPublishedDate.");
            }
            var searchKey = searchBookRequest.SearchKey;
            var authorId = searchBookRequest.AuthorId;

            var pageSize = searchBookRequest.PageSize;
            var pageIndex = searchBookRequest.PageIndex;

            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            try
            {
                // Use DbCommand to execute the stored procedure and get the data
                var command = connection.CreateCommand();
                command.CommandText = "EXEC GetBooksByFilters @SearchKey, @AuthorId, @FromPublishedDate, @ToPublishedDate, @PageSize, @PageIndex";
                command.Parameters.Add(new SqlParameter("@SearchKey", SqlDbType.NVarChar) { Value = (object)searchKey ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) { Value = (object)authorId ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@FromPublishedDate", SqlDbType.DateTime) { Value = (object)fromPublishedDate ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@ToPublishedDate", SqlDbType.DateTime) { Value = (object)toPublishedDate ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize });
                command.Parameters.Add(new SqlParameter("@PageIndex", SqlDbType.Int) { Value = pageIndex });

                var reader = await command.ExecuteReaderAsync();

                // First result set: Books
                var books = new List<BooksWithAuthors>();
                while (await reader.ReadAsync())
                {
                    var book = new BooksWithAuthors
                    {
                        BookId = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Price = reader.GetDecimal(2),
                        PublishedDate = reader.GetDateTime(3),
                        AuthorId = reader.GetInt32(4),
                        Name = reader.GetString(5),
                        Bio = reader.GetString(6)
                    };
                    books.Add(book);
                    Console.WriteLine(book.BookId);
                }

                // Move to the second result set: Total Count
                await reader.NextResultAsync();
                int totalCount = 0;
                if (await reader.ReadAsync())
                {
                    totalCount = reader.GetInt32(0);
                }

                // Map the data to BookResponse and create PaginatedList
                var bookResponses = _mapper.Map<List<BookResponse>>(books);
                var paginatedList = new PaginatedList<BookResponse>(bookResponses, totalCount, pageIndex, pageSize);

                return paginatedList;

            }
            catch (Exception ex)
            {
                // Log or handle the error
                throw new Exception("An error occurred while fetching the report data.", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }

        }
    }
}
