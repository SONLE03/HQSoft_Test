using HQSoft_EX01.Common.Pagination;
using HQSoft_EX01.Common;
using HQSoft_EX01.Constants;
using HQSoft_EX01.DTOs.Request;
using HQSoft_EX01.Services.AuthorService;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HQSoft_EX01.Controllers
{
    [ApiController]
    [Route(Routes.AUTHORS)]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        [HttpGet("fetch")]
        public async Task<IActionResult> GetAllAuthors([FromQuery] PageInfo pageInfo)
        {
            return new SuccessfulResponse<object>(await _authorService.GetAllAuthors(pageInfo), (int)HttpStatusCode.OK, "Get authors successfully").GetResponse();
        }
        [HttpGet("fetch/{authorId}")]
        public async Task<IActionResult> GetAuthorById(int authorId)
        {
            return new SuccessfulResponse<object>(await _authorService.GetAuthorById(authorId), (int)HttpStatusCode.OK, "Get author successfully").GetResponse();
        }
        [HttpPost("insert")]
        public async Task<IActionResult> CreateAuthor(AuthorRequest authorRequest)
        {
            return new SuccessfulResponse<object>(await _authorService.CreateAuthor(authorRequest), (int)HttpStatusCode.OK, "Your author has been successfully added").GetResponse();
        }
        [HttpPut("update/{authorId}")]
        public async Task<IActionResult> UpdateAuthor(int authorId, AuthorRequest authorRequest)
        {
            return new SuccessfulResponse<object>(await _authorService.UpdateAuthor(authorId, authorRequest), (int)HttpStatusCode.OK, "Your author has been successfully mpdified").GetResponse();
        }
        [HttpDelete("{authorId}")]
        public async Task<IActionResult> DeleteAuthor(int authorId)
        {
            await _authorService.DeleteAuthor(authorId);
            return new SuccessfulResponse<object>(null, (int)HttpStatusCode.OK, "Your author has been successfully deleted").GetResponse();
        }
    }
}
