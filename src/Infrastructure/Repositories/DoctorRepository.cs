using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinAgenda.src.Application.DTOs.Doctor;
using ClinAgenda.src.Core.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;

namespace ClinAgenda.src.Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly MySqlConnection _connection;

        public DoctorRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<DoctorDTO> GetByIdAsync(int id)
        {
            const string query = @"
                SELECT 
                    ID, 
                    NAME,
                    STATUSID, 
                FROM DOCTOR
                WHERE ID = @Id";

            var doctor = await _connection.QueryFirstOrDefaultAsync<DoctorDTO>(query, new { Id = id });

            return doctor;
        }

        public async Task<(int total, IEnumerable<DoctorListDTO> doctor)> GetPatientsAsync(string? name, int? statusId, int itemsPerPage, int page)
        {
            var queryBase = new StringBuilder(@"     
                    FROM DOCTOR P
                    INNER JOIN STATUS S ON S.ID = D.STATUSID
                    WHERE 1 = 1");

            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(name))
            {
                queryBase.Append(" AND P.NAME LIKE @Name");
                parameters.Add("Name", $"%{name}%");
            }

            if (statusId.HasValue)
            {
                queryBase.Append(" AND S.ID = @StatusId");
                parameters.Add("StatusId", statusId.Value);
            }

            var countQuery = $"SELECT COUNT(DISTINCT D.ID) {queryBase}";
            int total = await _connection.ExecuteScalarAsync<int>(countQuery, parameters);

            var dataQuery = $@"
                    SELECT 
                        D.ID, 
                        D.NAME,
                        D.STATUSID AS STATUSID, 
                        D.NAME AS STATUSNAME
                    {queryBase}
                    ORDER BY D.ID
                    LIMIT @Limit OFFSET @Offset";

            parameters.Add("Limit", itemsPerPage);
            parameters.Add("Offset", (page - 1) * itemsPerPage);

            var doctors = await _connection.QueryAsync<DoctorListDTO>(dataQuery, parameters);

            return (total, doctors);
        }
        public async Task<int> InsertDoctorAsync(DoctorInsertDTO doctor)
        {
            string query = @"
            INSERT INTO Doctor (name, statusId) 
            VALUES (@Name, @StatusId);
            SELECT LAST_INSERT_ID();";
            return await _connection.ExecuteScalarAsync<int>(query, doctor);
        }
        public async Task<bool> UpdateAsync(DoctorDTO doctor)
        {
            string query = @"
            UPDATE Doctor SET 
                Name = @Name,
                StatusId = @StatusId
            WHERE Id = @Id;";
            int rowsAffected = await _connection.ExecuteAsync(query, doctor);
            return rowsAffected > 0;
        }
        public async Task<int> DeleteByDoctorIdAsync(int id)
        {
            string query = "DELETE FROM Doctor WHERE ID = @Id";

            var parameters = new { Id = id };

            var rowsAffected = await _connection.ExecuteAsync(query, parameters);

            return rowsAffected;
        }
    }
}