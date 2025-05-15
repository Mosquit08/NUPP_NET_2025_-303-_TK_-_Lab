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
    public class ReadersController : ControllerBase
    {
        private readonly ICrudServiceAsync<ReaderModel> _readerService;

        public ReadersController(ICrudServiceAsync<ReaderModel> readerService)
        {
            _readerService = readerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReaderModel>>> GetReaders([FromQuery] int? page, [FromQuery] int? amount)
        {
            try
            {
                if (page.HasValue && amount.HasValue)
                {
                    return Ok(await _readerService.ReadAllAsync(page.Value, amount.Value));
                }
                return Ok(await _readerService.ReadAllAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReaderModel>> GetReader(Guid id)
        {
            try
            {
                var reader = await _readerService.ReadAsync(id);
                if (reader == null)
                {
                    return NotFound();
                }
                return Ok(reader);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ReaderModel>> CreateReader(ReaderModel reader)
        {
            try
            {
                if (await _readerService.CreateAsync(reader))
                {
                    await _readerService.SaveAsync();
                    return CreatedAtAction(nameof(GetReader), new { id = reader.Id }, reader);
                }
                return BadRequest("Не удалось создать читателя");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReader(Guid id, ReaderModel reader)
        {
            try
            {
                if (id != reader.Id)
                {
                    return BadRequest("ID в URL не соответствует ID в теле запроса");
                }

                var existingReader = await _readerService.ReadAsync(id);
                if (existingReader == null)
                {
                    return NotFound();
                }

                if (await _readerService.UpdateAsync(reader))
                {
                    await _readerService.SaveAsync();
                    return NoContent();
                }
                return BadRequest("Не удалось обновить читателя");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReader(Guid id)
        {
            try
            {
                var reader = await _readerService.ReadAsync(id);
                if (reader == null)
                {
                    return NotFound();
                }

                if (await _readerService.RemoveAsync(reader))
                {
                    await _readerService.SaveAsync();
                    return NoContent();
                }
                return BadRequest("Не удалось удалить читателя");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
    }
} 