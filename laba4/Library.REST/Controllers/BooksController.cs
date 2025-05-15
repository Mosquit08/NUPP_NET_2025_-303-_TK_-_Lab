using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library.REST.Models;
using Library.REST.Services;

namespace Library.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ICrudServiceAsync<BookModel> _bookService;

        public BooksController(ICrudServiceAsync<BookModel> bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookModel>>> GetBooks([FromQuery] int? page, [FromQuery] int? amount)
        {
            try
            {
                if (page.HasValue && amount.HasValue)
                {
                    return Ok(await _bookService.ReadAllAsync(page.Value, amount.Value));
                }
                return Ok(await _bookService.ReadAllAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookModel>> GetBook(Guid id)
        {
            try
            {
                var book = await _bookService.ReadAsync(id);
                if (book == null)
                {
                    return NotFound();
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<BookModel>> CreateBook(BookModel book)
        {
            try
            {
                if (await _bookService.CreateAsync(book))
                {
                    await _bookService.SaveAsync();
                    return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
                }
                return BadRequest("Не удалось создать книгу");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, BookModel book)
        {
            try
            {
                if (id != book.Id)
                {
                    return BadRequest("ID в URL не соответствует ID в теле запроса");
                }

                var existingBook = await _bookService.ReadAsync(id);
                if (existingBook == null)
                {
                    return NotFound();
                }

                if (await _bookService.UpdateAsync(book))
                {
                    await _bookService.SaveAsync();
                    return NoContent();
                }
                return BadRequest("Не удалось обновить книгу");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            try
            {
                var book = await _bookService.ReadAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

                if (await _bookService.RemoveAsync(book))
                {
                    await _bookService.SaveAsync();
                    return NoContent();
                }
                return BadRequest("Не удалось удалить книгу");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
    }
} 