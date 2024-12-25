using AutoMapper;
using HQSoft_EX01.DTOs.Response;
using HQSoft_EX01.Models;

namespace HQSoft_EX01.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Authors, AuthorResponse>();
            CreateMap<Books, BooksWithAuthors>()
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Author.Name))
             .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Author.Bio));
            CreateMap<BooksWithAuthors, BookResponse>()
              .ForMember(dest => dest.AuthorResponse, opt => opt.MapFrom(src => new AuthorResponse
              {
                  AuthorId = src.AuthorId,
                  Name = src.Name,
                  Bio = src.Bio
              }));
            CreateMap<Books, BookResponse>()
                .ForMember(dest => dest.AuthorResponse, opt => opt.MapFrom(src => new AuthorResponse
                {
                    AuthorId = src.AuthorId,
                    Name = src.Author.Name,
                    Bio = src.Author.Bio
                }));
        }
    }
}
