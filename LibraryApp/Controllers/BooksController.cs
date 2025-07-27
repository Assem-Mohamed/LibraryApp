using LibraryApp.Data.Entities;
using LibraryApp.Data.Enums;
using LibraryApp.Data.Interfaces;
using LibraryApp.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookRepository.GetAllAsync();
            var response = books.Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Description = b.Description,
                Category = b.Category,
                CoverImageUrl = b.CoverImageUrl,
                BookStatus = b.BookStatus
            });
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                return NotFound();

            return Ok(new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Category = book.Category,
                CoverImageUrl = book.CoverImageUrl,
                BookStatus = book.BookStatus
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> Create(CreateBookDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Description = dto.Description,
                Category = dto.Category,
                CoverImageUrl = dto.CoverImageUrl,
                BookStatus = BookStatus.Available
            };
            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();
            return Ok("Book created successfully.");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> Update(int id, UpdateBookDto dto)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                return NotFound();

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.BookStatus = dto.BookStatus;

            if (!string.IsNullOrWhiteSpace(dto.Description))
            {
                book.Description = dto.Description;
            }
            if (!string.IsNullOrWhiteSpace(dto.Category))
            {
                book.Category = dto.Category;
            }
            if (!string.IsNullOrWhiteSpace(dto.CoverImageUrl))
            {
                book.CoverImageUrl = dto.CoverImageUrl;
            }



            await _bookRepository.UpdateAsync(book);
            await _bookRepository.SaveChangesAsync();
            return Ok("Book updated successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles   = "Admin, Librarian")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookRepository.DeleteAsync(id);
            await _bookRepository.SaveChangesAsync();
            return Ok("Book deleted successfully.");
        }
        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> Search(string? title, string? author, string? category)
        {
            var books = await _bookRepository.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(title))
                books = books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(author))
                books = books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(category))
                books = books.Where(b => b.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

            var response = books.Select(b => new BookResponseDto {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Description = b.Description,
                Category = b.Category,
                CoverImageUrl = b.CoverImageUrl,
                BookStatus = b.BookStatus
            });
            return Ok(response);
        }
    }
}
