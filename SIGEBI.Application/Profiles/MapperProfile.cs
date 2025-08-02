using AutoMapper;
using SIGEBI.Domain.Entities;
using SIGEBI.Application.DTOs;

namespace SIGEBI.Application.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<CreateBookDto, Book>();
            CreateMap<UpdateBookDto, Book>();

            CreateMap<Author, AuthorDto>().ReverseMap();
            CreateMap<CreateAuthorDto, Author>();
            CreateMap<UpdateAuthorDto, Author>();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            CreateMap<BookAuthor, BookAuthorDto>().ReverseMap();
            CreateMap<CreateBookAuthorDto, BookAuthor>();
            CreateMap<UpdateBookAuthorDto, BookAuthor>();

            CreateMap<Loan, LoanDto>().ReverseMap();
            CreateMap<CreateLoanDto, Loan>();
            CreateMap<UpdateLoanDto, Loan>();

            CreateMap<LoanHistory, LoanHistoryDto>().ReverseMap();
            CreateMap<CreateLoanHistoryDto, LoanHistory>();
            CreateMap<UpdateLoanHistoryDto, LoanHistory>();
        }
    }
}
