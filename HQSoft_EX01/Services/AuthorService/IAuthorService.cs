using HQSoft_EX01.Common.Pagination;
using HQSoft_EX01.DTOs.Request;
using HQSoft_EX01.DTOs.Response;

namespace HQSoft_EX01.Services.AuthorService
{
    public interface IAuthorService
    {
        Task<PaginatedList<AuthorResponse>> GetAllAuthors(PageInfo pageInfo);
        Task<AuthorResponse> GetAuthorById(int authorId);
        Task<AuthorResponse> CreateAuthor(AuthorRequest authorRequest);
        Task<AuthorResponse> UpdateAuthor(int authorId, AuthorRequest authorRequest);
        Task DeleteAuthor(int authorId);
    }
}
