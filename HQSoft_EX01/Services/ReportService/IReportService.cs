using HQSoft_EX01.Common.Pagination;
using HQSoft_EX01.DTOs.Request;
using HQSoft_EX01.DTOs.Response;

namespace HQSoft_EX01.Services.ReportService
{
    public interface IReportService
    {
        Task<PaginatedList<BookResponse>> GetReportsBook(SearchBookRequest searchBookRequest);
    }
}
