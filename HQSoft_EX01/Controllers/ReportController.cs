using HQSoft_EX01.Common;
using HQSoft_EX01.Common.Pagination;
using HQSoft_EX01.Constants;
using HQSoft_EX01.DTOs.Request;
using HQSoft_EX01.Services.ReportService;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HQSoft_EX01.Controllers
{
    [ApiController]
    [Route(Routes.REPORTS)]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet("book")]
        public async Task<IActionResult> GetBooksByFilters([FromQuery] SearchBookRequest searchBookRequest)
        {
            return new SuccessfulResponse<object>(await _reportService.GetReportsBook(searchBookRequest), (int)HttpStatusCode.OK, "Get books successfully").GetResponse();
        }
    }
}
