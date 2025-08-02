using AutoMapper;
using SIGEBI.Application.DTOs;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto?> GetByIdAsync(int id)
        {
            var book = await _bookRepository.GetEntityByIdAsync(id);
            return book == null ? null : _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> CreateAsync(CreateBookDto dto)
        {
            var entity = _mapper.Map<Book>(dto);
            var result = await _bookRepository.SaveEntityAsync(entity);

            if (!result.Success || result.Data is not Book createdBook)
                throw new Exception("Error al crear el libro: " + result.Message);

            return _mapper.Map<BookDto>(createdBook);
        }

        public async Task<bool> UpdateAsync(UpdateBookDto dto)
        {
            var exists = await _bookRepository.Exists(b => b.ID == dto.Id);
            if (!exists) return false;

            var entity = _mapper.Map<Book>(dto);
            var result = await _bookRepository.UpdateEntityAsync(entity);
            return result.Success;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _bookRepository.GetEntityByIdAsync(id);
            if (book is null) return false;

            var result = await _bookRepository.RemoveEntityAsync(book);
            return result.Success;
        }
    }
}
