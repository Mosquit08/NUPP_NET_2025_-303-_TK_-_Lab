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
    public class BorrowingRecordsController : ControllerBase
    {
        private readonly ICrudServiceAsync<BorrowingRecordModel> _borrowingRecordService;

        public BorrowingRecordsController(ICrudServiceAsync<BorrowingRecordModel> borrowingRecordService)
        {
            _borrowingRecordService = borrowingRecordService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowingRecordModel>>> GetBorrowingRecords([FromQuery] int? page, [FromQuery] int? amount)
        {
            try
            {
                if (page.HasValue && amount.HasValue)
                {
                    return Ok(await _borrowingRecordService.ReadAllAsync(page.Value, amount.Value));
                }
                return Ok(await _borrowingRecordService.ReadAllAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowingRecordModel>> GetBorrowingRecord(Guid id)
        {
            try
            {
                var record = await _borrowingRecordService.ReadAsync(id);
                if (record == null)
                {
                    return NotFound();
                }
                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<BorrowingRecordModel>> CreateBorrowingRecord(BorrowingRecordModel record)
        {
            try
            {
                record.BorrowDate = DateTime.UtcNow;
                if (await _borrowingRecordService.CreateAsync(record))
                {
                    await _borrowingRecordService.SaveAsync();
                    return CreatedAtAction(nameof(GetBorrowingRecord), new { id = record.Id }, record);
                }
                return BadRequest("Не удалось создать запись выдачи");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBorrowingRecord(Guid id, BorrowingRecordModel record)
        {
            try
            {
                if (id != record.Id)
                {
                    return BadRequest("ID в URL не соответствует ID в теле запроса");
                }

                var existingRecord = await _borrowingRecordService.ReadAsync(id);
                if (existingRecord == null)
                {
                    return NotFound();
                }

                if (record.IsReturned && !existingRecord.IsReturned)
                {
                    record.ReturnDate = DateTime.UtcNow;
                }

                if (await _borrowingRecordService.UpdateAsync(record))
                {
                    await _borrowingRecordService.SaveAsync();
                    return NoContent();
                }
                return BadRequest("Не удалось обновить запись выдачи");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrowingRecord(Guid id)
        {
            try
            {
                var record = await _borrowingRecordService.ReadAsync(id);
                if (record == null)
                {
                    return NotFound();
                }

                if (await _borrowingRecordService.RemoveAsync(record))
                {
                    await _borrowingRecordService.SaveAsync();
                    return NoContent();
                }
                return BadRequest("Не удалось удалить запись выдачи");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
    }
} 