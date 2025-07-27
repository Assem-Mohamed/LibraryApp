using LibraryApp.Models.DTOs;
using LibraryApp.Data.Entities;
using LibraryApp.Data.Enums;
using LibraryApp.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BorrowRecordsController : ControllerBase
    {
        private readonly IBorrowRecordRepository _borrowRecordRepository;
        private readonly IBookRepository _bookRepository;

        public BorrowRecordsController(IBorrowRecordRepository borrowRecordRepository, IBookRepository bookRepository)
        {
            _borrowRecordRepository = borrowRecordRepository;
            _bookRepository = bookRepository;
        }

    [HttpPost]
    public async Task<IActionResult> BorrowBook(CreateBorrowRecordDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var book = await _bookRepository.GetByIdAsync(dto.BookId);
        if (book == null || book.BookStatus != BookStatus.Available)
            return BadRequest("Book is not available for borrowing.");

        var borrowRecord = new BorrowRecord
        {
            UserId = userId,
            BookId = dto.BookId,
            BorrowDate = dto.BorrowDate,
            DueDate = dto.DueDate,
            BorrowStatus = BorrowStatus.Active
        };

        book.BookStatus = BookStatus.Borrowed;

        await _borrowRecordRepository.AddAsync(borrowRecord);
        await _borrowRecordRepository.SaveChangesAsync();
        await _bookRepository.UpdateAsync(book);
        await _bookRepository.SaveChangesAsync();

        return Ok("Book borrowed successfully.");
    }

    [HttpPut("{id}/return")]
    public async Task<IActionResult> ReturnBook(int id, ReturnBookDto dto)
    {
        var borrowRecord = await _borrowRecordRepository.GetByIdAsync(id);
        if (borrowRecord == null || borrowRecord.BorrowStatus != BorrowStatus.Active)
            return BadRequest("Borrow record not found or already returned.");

        var book = await _bookRepository.GetByIdAsync(borrowRecord.BookId);
        if (book == null)
            return BadRequest("Associated book not found.");

        borrowRecord.ReturnDate = dto.ReturnDate;
        borrowRecord.BorrowStatus = BorrowStatus.Returned;
        book.BookStatus = BookStatus.Available;

        await _borrowRecordRepository.UpdateAsync(borrowRecord);
        await _borrowRecordRepository.SaveChangesAsync();
        await _bookRepository.UpdateAsync(book);
        await _bookRepository.SaveChangesAsync();

        return Ok("Book returned successfully.");
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetBorrowHistory()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var records = await _borrowRecordRepository.GetByUserIdAsync(userId);

        var response = records.Select(r => new BorrowRecordDto
        {
            Id = r.Id,
            BookTitle = r.Book.Title,
            BorrowDate = r.BorrowDate,
            DueDate = r.DueDate,
            ReturnDate = r.ReturnDate,
            BorrowStatus = r.BorrowStatus
        });
        return Ok(response);
    }

    //[Authorize(Roles = "Admin, Librarian")]
    [HttpGet("overdue")]
    public async Task<IActionResult> GetOverdueRecords()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var overdueRecords = await _borrowRecordRepository.GetOverdueRecordsAsync(today);

        var response = overdueRecords.Select(r => new BorrowRecordDto
        {
            Id = r.Id,
            BookTitle = r.Book.Title,
            BorrowDate = r.BorrowDate,
            DueDate = r.DueDate,
            ReturnDate = r.ReturnDate,
            BorrowStatus = r.BorrowStatus
        });
        return Ok(response);
    }


    }
}
