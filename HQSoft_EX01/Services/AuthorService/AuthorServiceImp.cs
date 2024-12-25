using AutoMapper;
using HQSoft_EX01.Common.Pagination;
using HQSoft_EX01.Data;
using HQSoft_EX01.DTOs.Request;
using HQSoft_EX01.DTOs.Response;
using HQSoft_EX01.Exceptions;
using HQSoft_EX01.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HQSoft_EX01.Services.AuthorService
{
    public class AuthorServiceImp : IAuthorService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        public AuthorServiceImp(ApplicationDBContext context, IMapper map)
        {
            _context = context;
            _mapper = map;
        }

        public async Task<AuthorResponse> CreateAuthor(AuthorRequest authorRequest)
        {
            var author = new Authors
            {
                Name = authorRequest.Name,
                Bio = authorRequest.Bio
            };
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return _mapper.Map<AuthorResponse>(author);
        }

        public async Task DeleteAuthor(int authorId)
        {
            // Kiểm tra theo Id dùng @Param để xóa bản ghi
            var author = await _context.Authors.FindAsync(authorId);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ObjectNotFoundException("Author not found");
            }
        }

        public async Task<PaginatedList<AuthorResponse>> GetAllAuthors(PageInfo pageInfo)
        {
            var authors = await _context.Authors
                .FromSqlRaw("EXEC GetAuthorsPaged @PageNumber = {0}, @PageSize = {1}", pageInfo.PageNumber, pageInfo.PageSize)
                .ToListAsync();

            // Ánh xạ dữ liệu từ Author sang AuthorResponse
            var authorResponses = _mapper.Map<List<AuthorResponse>>(authors);

            // Tạo đối tượng PaginatedList
            var totalCount = await _context.Authors.CountAsync(); // Lấy tổng số tác giả trong bảng
            var paginatedList = new PaginatedList<AuthorResponse>(authorResponses, totalCount, pageInfo.PageNumber, pageInfo.PageSize);

            return paginatedList;
        }

        public async Task<AuthorResponse> GetAuthorById(int authorId)
        {
            // Gọi stored procedure GetAuthorById
            var authors = await _context.Authors
                .FromSqlRaw("EXEC GetAuthorById @AuthorId", new SqlParameter("@AuthorId", authorId))
                .ToListAsync();  // Lấy danh sách tác giả (dù chỉ có 1 hoặc 0)

            var author = authors.FirstOrDefault();  // Lấy tác giả đầu tiên hoặc null

            if (author == null)
            {
                throw new ObjectNotFoundException("Author not found");
            }

            return _mapper.Map<AuthorResponse>(author);
        }




        public async Task<AuthorResponse> UpdateAuthor(int authorId, AuthorRequest authorRequest)
        {
            var author = await _context.Authors.FindAsync(authorId);
            if (author != null)
            {
                author.Name = authorRequest.Name;
                author.Bio = authorRequest.Bio;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ObjectNotFoundException("Author not found");

            }
            return _mapper.Map<AuthorResponse>(author);
        }
    }
}
