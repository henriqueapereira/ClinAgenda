using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinAgenda.src.Application.DTOs.Specialty;
using ClinAgenda.src.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ClinAgenda.src.WebApi.Controllers
{
    [ApiController]
    [Route("api/specialty")]
    public class SpecialtyController : ControllerBase
    {
        private readonly SpecialtyUseCase _specialtyUsecase;
        public SpecialtyController(SpecialtyUseCase service)
        {
            _specialtyUsecase = service;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetSpecialtyAsync([FromQuery] int itemsPerPage = 10, [FromQuery] int page = 1)
        {
            try
            {
                var specialty = await _specialtyUsecase.GetSpecialtyAsync(itemsPerPage, page);
                return Ok(specialty);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
        [HttpPost("insert")]
        public async Task<IActionResult> CreateSpecialtyAsync([FromBody] SpecialtyInsertDTO specialty)
        {
            try
            {
                if (specialty == null)
                {
                    return BadRequest("Dados inválidos para criação de especialidade.");
                }

                var createdSpecialty = await _specialtyUsecase.CreateSpecialtyAsync(specialty);

                if (!(createdSpecialty > 0))
                {
                    return StatusCode(500, "Erro ao criar a especialidade.");
                }

                var infosSpecialtyCreated = await _specialtyUsecase.GetSpecialtyByIdAsync(createdSpecialty);
                return Ok(infosSpecialtyCreated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
        [HttpGet("listById/{id}")]
        public async Task<IActionResult> GetSpecialtyByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("ID inválido.");
                }

                var specialty = await _specialtyUsecase.GetSpecialtyByIdAsync(id);
                if (specialty == null)
                {
                    return NotFound($"Especialidade com ID {id} não encontrada.");
                }

                return Ok(specialty);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
    }
}