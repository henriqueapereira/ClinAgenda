using System.ComponentModel.DataAnnotations;

namespace ClinAgenda.src.Application.DTOs.Doctor
{
    public class DoctorInsertDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage="O nome do Doutor é obrigatório",AllowEmptyStrings=false)]
        public required string Name { get; set; }
        public required int StatusId { get; set; }
    }
}