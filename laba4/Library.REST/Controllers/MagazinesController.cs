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
    public class MagazinesController : ControllerBase
    {
        private readonly ICrudServiceAsync<MagazineModel> _magazineService;

        public MagazinesController(ICrudServiceAsync<MagazineModel> magazineService)
        {
            _magazineService = magazineService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MagazineModel>>> GetMagazines([FromQuery] int? page, [FromQuery] int? amount)
        {
            try
            {
                if (page.HasValue && amount.HasValue)
                {
                    return Ok(await _magazineService.ReadAllAsync(page.Value, amount.Value));
                }
                return Ok(await _magazineService.ReadAllAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MagazineModel>> GetMagazine(Guid id)
        {
            try
            {
                var magazine = await _magazineService.ReadAsync(id);
                if (magazine == null)
                {
                    return NotFound();
                }
                return Ok(magazine);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<MagazineModel>> CreateMagazine(MagazineModel magazine)
        {
            try
            {
                if (await _magazineService.CreateAsync(magazine))
                {
                    await _magazineService.SaveAsync();
                    return CreatedAtAction(nameof(GetMagazine), new { id = magazine.Id }, magazine);
                }
                return BadRequest("Не удалось создать журнал");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMagazine(Guid id, MagazineModel magazine)
        {
            try
            {
                if (id != magazine.Id)
                {
                    return BadRequest("ID в URL не соответствует ID в теле запроса");
                }

                var existingMagazine = await _magazineService.ReadAsync(id);
                if (existingMagazine == null)
                {
                    return NotFound();
                }

                if (await _magazineService.UpdateAsync(magazine))
                {
                    await _magazineService.SaveAsync();
                    return NoContent();
                }
                return BadRequest("Не удалось обновить журнал");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMagazine(Guid id)
        {
            try
            {
                var magazine = await _magazineService.ReadAsync(id);
                if (magazine == null)
                {
                    return NotFound();
                }

                if (await _magazineService.RemoveAsync(magazine))
                {
                    await _magazineService.SaveAsync();
                    return NoContent();
                }
                return BadRequest("Не удалось удалить журнал");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
    }
} 