using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinAgenda.src.Application.DTOs.Patient;
using Dapper;
using MySql.Data.MySqlClient;

namespace ClinAgenda.src.Infrastructure.Repositories
{
    public class PatientRepository
    {
        private readonly MySqlConnection _connection;

        public PatientRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<PatientDTO> GetByIdAsync(int id)
        {
            const string query = @"
                SELECT 
                    ID, 
                    NAME,
                    PHONENUMBER,
                    DOCUMENTNUMBER,
                    STATUSID,
                    BIRTHDATE 
                FROM PATIENT
                WHERE ID = @Id";

            var patient = await _connection.QueryFirstOrDefaultAsync<PatientDTO>(query, new { Id = id });

            return patient;
        }

        public async Task<(int total, IEnumerable<PatientDTO> specialtys)> GetAllAsync(int? itemsPerPage, int? page)
        {
            var queryBase = new StringBuilder(@"
                FROM PATIENT S WHERE 1 = 1");

            var parameters = new DynamicParameters();

            var countQuery = $"SELECT COUNT(DISTINCT S.ID) {queryBase}";
            int total = await _connection.ExecuteScalarAsync<int>(countQuery, parameters);

            var dataQuery = $@"
            SELECT ID, 
            NAME, 
            SCHEDULEDURATION
            {queryBase}
            LIMIT @Limit OFFSET @Offset";

            parameters.Add("Limit", itemsPerPage);
            parameters.Add("Offset", (page - 1) * itemsPerPage);

            var specialtys = await _connection.QueryAsync<SpecialtyDTO>(dataQuery, parameters);

            return (total, specialtys);
        }
    }
}