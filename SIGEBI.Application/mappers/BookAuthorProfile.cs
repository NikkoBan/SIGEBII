using AutoMapper;
using SIGEBI.Application.Dtos;
using SIGEBI.Domain.Entities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.mappers
{
    public class BookAuthorProfile : Profile
    {
        public BookAuthorProfile()
        {
            CreateMap<CreateBookAuthorDTO, BooksAuthors>();
            CreateMap<UpdateBookAuthorDTO, BooksAuthors>(); 
            CreateMap<BooksAuthors, BookAuthorDTO>();
        }
    }
}
