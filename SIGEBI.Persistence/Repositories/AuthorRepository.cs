using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Contracts.Interface;
using SIGEBI.Application.Contracts.Repository;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;
using SIGEBI.Persistence.Repositories.SIGEBI.Persistence.Repositories;
using System.Data;
using System.Linq.Expressions;



namespace SIGEBI.Persistence.Repositories
{


    public class AuthorRepository : BaseRepositoryAdo<Authors>, IAuthorRepository
    {
        public AuthorRepository(string connectionString, ILogger<BaseRepositoryAdo<Authors>> logger)
            : base(connectionString, logger) { }

        public override async Task<OperationResult> CreateAsync(Authors entity)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = new SqlCommand("CreateAuthor", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                       
                        cmd.Parameters.AddWithValue("@FirstName", entity.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", entity.LastName);
                        cmd.Parameters.AddWithValue("@BirthDate", (object?)entity.BirthDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Nationality", entity.Nationality);
                        cmd.Parameters.AddWithValue("@CreatedBy", entity.CreatedBy ?? "System");

                        
                        var result = await cmd.ExecuteScalarAsync();

                        
                        if (result != null && int.TryParse(result.ToString(), out int newId))
                        {
                            entity.AuthorId = newId;
                        }

                        return new OperationResult { Success = true, Data = entity };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el autor.");
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public override async Task<OperationResult> GetByIdAsync(int id)
        {
            
            try { /* ... */ return new OperationResult { Success = true, Data = new Authors() }; }
            catch (Exception ex) { return new OperationResult { Success = false, Message = ex.Message }; }
        }

        public override async Task<OperationResult> GetAllAsync()
        {
            
            try { /* ... */ return new OperationResult { Success = true, Data = new List<Authors>() }; }
            catch (Exception ex) { return new OperationResult { Success = false, Message = ex.Message }; }
        }

        public override async Task<OperationResult> UpdateAsync(Authors entity)
        {
           
            try { /* ... */ return new OperationResult { Success = true, Data = entity }; }
            catch (Exception ex) { return new OperationResult { Success = false, Message = ex.Message }; }
        }

        public override async Task<OperationResult> DeleteAsync(int id)
        {
            
            try { /* ... */ return new OperationResult { Success = true }; }
            catch (Exception ex) { return new OperationResult { Success = false, Message = ex.Message }; }
        }

        public override async Task<bool> ExistsAsync(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand("SELECT COUNT(1) FROM Authors WHERE AuthorId = @AuthorId", conn))
                    {
                        cmd.Parameters.AddWithValue("@AuthorId", id);
                        return Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al verificar existencia de autor con ID {id}.");
                return false;
            }
        }

        public async Task<bool> CheckDuplicateAuthorAsync(string firstName, string lastName, DateTime? birthDate)
        {
           
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string sql = "SELECT COUNT(1) FROM Authors WHERE FirstName = @FirstName AND LastName = @LastName";
                    if (birthDate.HasValue) sql += " AND BirthDate = @BirthDate";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        if (birthDate.HasValue) cmd.Parameters.AddWithValue("@BirthDate", birthDate.Value);
                        else cmd.Parameters.AddWithValue("@BirthDate", DBNull.Value); 

                        return Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar duplicidad de autor.");
                return false;
            }
        }

        public async Task<bool> CheckDuplicateAuthorForUpdateAsync(int id, string firstName, string lastName, DateTime? birthDate)
        {
            
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string sql = "SELECT COUNT(1) FROM Authors WHERE AuthorId != @AuthorId AND FirstName = @FirstName AND LastName = @LastName";
                    if (birthDate.HasValue) sql += " AND BirthDate = @BirthDate";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@AuthorId", id);
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        if (birthDate.HasValue) cmd.Parameters.AddWithValue("@BirthDate", birthDate.Value);
                        else cmd.Parameters.AddWithValue("@BirthDate", DBNull.Value);

                        return Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al verificar duplicidad de autor para actualización de ID {id}.");
                return false;
            }
        }

        public override Task<OperationResult> CreateAsync(AuditableEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}

