using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinAgenda.src.Core.Entities
{
    public class Status
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}