using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinAgenda.src.Application.DTOs.Patient;
using ClinAgenda.src.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ClinAgenda.src.WebApi.Controllers
{
    [ApiController]
    []
    public class PatientController : ControllerBase
    {
        private readonly PatientUseCase _patientUseCase;

        private readonly StatusUseCase _statusUseCase;

        public PatientController(PatientUseCase service, StatusUseCase statusUseCase)
        {
            _patientUseCase = service;
            _statusUseCase = statusUseCase;
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetPatientsAsync([FromQuery] string? name, [FromQuery] string? documentNumber, [FromQuery] int? statusId, [FromQuery] int itemsPerPage = 10, [FromQuery] int page = 1)
        {
            try
            {
                var result = await _patientUseCase.GetPatientsAsync(name, documentNumber, statusId, itemsPerPage, page);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPost("insert")]
        public async Task<IActionResult> CreateStatusAsync([FromBody] PatientInsertDTO patient)
        {
            try
            {
                var hasStatus = await _statusUseCase.GetStatusByIdAsync(patient.StatusId);
                if (hasStatus == null)
                    return BadRequest($"O status ID {patient.StatusId} não existe");

                var createdPatientId = await _patientUseCase.CreatePatientAsync(patient);

                if (!(createdPatientId > 0))
                {
                    return StatusCode(500, "Erro ao criar a Paciente.");
                }
                var infosPatientCreated = await _patientUseCase.GetPatientByIdAsync(createdPatientId);

                return Ok(infosPatientCreated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do Servidor: {ex.Message}");
            }
        }
        [HttpGet("listById/{id}")]
        public async Task<IActionResult> GetPatientByIdAsync(int id)
        {
            try
            {
                var doctor = await _patientUseCase.GetPatientByIdAsync(id);
                if (doctor == null) return NotFound();
                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do Servidor: {ex.Message}");
            }
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateDoctorAsync(int id, [FromBody] PatientInsertDTO patient)
        {
            try
            {
                if (patient == null) return BadRequest();

                var hasStatus = await _statusUseCase.GetStatusByIdAsync(patient.StatusId);
                if (hasStatus == null)
                    return BadRequest($"O status ID {patient.StatusId} não existe");

                bool updated = await _patientUseCase.UpdatePatientAsync(id, patient);
                if (!updated) return NotFound("Paciente não encontrado.");

                var infosDoctorUpdate = await _patientUseCase.GetPatientByIdAsync(id);
                return Ok(infosDoctorUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do Servidor: {ex.Message}");
            }
        }


    }
}