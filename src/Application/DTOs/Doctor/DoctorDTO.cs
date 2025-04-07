using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinAgenda.src.Application.DTOs.Doctor
{
    public class DoctorDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int StatusId { get; set; }
    }
}