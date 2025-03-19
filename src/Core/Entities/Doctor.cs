using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinAgenda.src.Core.Entities
{
    public class Doctor
    {
        private int Id { get; set; }
        public required string Name { get; set; }
        public int StatusId { get; set; }

    }
}