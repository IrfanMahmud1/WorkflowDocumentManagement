using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDM.Domain.Entities;
using WDM.Domain.Repositories;

namespace WDM.Infrastructure.Repositories
{
    public class DocumentTypeRepository : IDocumentTypeRepository
    {
        private readonly string _connectionString;
        private const string StoredProcedureName = "sp_DocumentType_Operations";

        public DocumentTypeRepository( IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<IEnumerable<DocumentType>> GetAllAsync()
        {
            var users = new List<DocumentType>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add(new SqlParameter("@Action", "GET_ALL"));

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(MapUserFromReader(reader));
            }

            return users;
        }

        public async Task<DocumentType> GetByIdAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add(new SqlParameter("@Action", "GET_BY_ID"));
            command.Parameters.Add(new SqlParameter("@Id", id));

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            return await reader.ReadAsync() ? MapUserFromReader(reader) : null;
        }

        public async Task<DocumentType> GetByNameAsync(string name)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add(new SqlParameter("@Action", "GET_BY_NAME"));
            command.Parameters.Add(new SqlParameter("@Name", name ?? (object)DBNull.Value));

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            return await reader.ReadAsync() ? MapUserFromReader(reader) : null;
        }

        public async Task<bool> AddAsync(DocumentType user)
        {
            var person = await GetByNameAsync(user.Name);
            if (person != null)
            {
                return false; // Email already exists for another user
            }
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddRange(new[]
            {
            new SqlParameter("@Action", "INSERT"),
            new SqlParameter("@Id", user.Id),
            new SqlParameter("@Name", user.Name ?? (object)DBNull.Value),
            new SqlParameter("@Description", user.Description ?? (object)DBNull.Value),
            new SqlParameter("@CreatedDate", user.CreatedDate),
            new SqlParameter("@CreatedBy", user.CreatedBy),
            new SqlParameter("@IsActive", user.IsActive)
        });

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(DocumentType user)
        {
            if (await ExistsAsync(user.Id,user.Name))
            {
                return false; // Email already exists for another user
            }
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddRange(new[]
            {
            new SqlParameter("@Action", "UPDATE"),
            new SqlParameter("@Id", user.Id),
            new SqlParameter("@Name", user.Name ?? (object)DBNull.Value),
            new SqlParameter("@Description", user.Description ?? (object)DBNull.Value),
            new SqlParameter("@IsActive", user.IsActive)
        });

            await connection.OpenAsync();
            var rowsAffected = await command.ExecuteScalarAsync();
            await connection.CloseAsync();
            return rowsAffected != null && Convert.ToInt32(rowsAffected) > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddRange(new[]
            {
            new SqlParameter("@Action", "DELETE"),
            new SqlParameter("@Id", id)
        });

            await connection.OpenAsync();
            var rowsAffected = await command.ExecuteScalarAsync();
            await connection.CloseAsync();

            return rowsAffected != null && Convert.ToInt32(rowsAffected) > 0;
        }

        public async Task<bool> ExistsAsync(Guid id,string name)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add(new SqlParameter("@Action", "EXISTS"));
            command.Parameters.Add(new SqlParameter("@Id", id));
            command.Parameters.Add(new SqlParameter("@Name", name ?? (object)DBNull.Value));

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            await connection.CloseAsync();

            return result != null && (int)result == 1;
        }

        private static DocumentType MapUserFromReader(SqlDataReader reader)
        {
            return new DocumentType
            {
                Id = reader.GetGuid("Id"),
                Name = reader.IsDBNull("Name") ? null : reader.GetString("Name"),
                Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                CreatedDate = reader.GetDateTime("CreatedDate"),
                CreatedBy = reader.GetGuid("CreatedBy"),
                IsActive = reader.GetBoolean("IsActive")
            };
        }

    }
}
