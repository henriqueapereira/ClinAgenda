using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinAgenda.src.Core.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        public required string DocumentNumber { get; set; }
        public required int StatusId { get; set; }
        public required string BirthDateday { get; set; }

    }
}