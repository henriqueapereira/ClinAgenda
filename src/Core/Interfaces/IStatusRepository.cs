using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinAgenda.src.Application.DTOs.Status;

namespace ClinAgenda.src.Core.Interfaces
{
    public interface IStatusRepository
    {
        Task<StatusDTO> GetByIdAsync(int id);
    }
}