using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDM.Domain.Entities;
using WDM.Domain.Repositories;

namespace WDM.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private const string StoredProcedureName = "sp_User_Management";

        public UserRepository( IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = new List<User>();

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

        public async Task<User> GetByIdAsync(Guid id)
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

        public async Task<User> GetByEmailAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add(new SqlParameter("@Action", "GET_BY_EMAIL"));
            command.Parameters.Add(new SqlParameter("@Email", email ?? (object)DBNull.Value));

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            return await reader.ReadAsync() ? MapUserFromReader(reader) : null;
        }

        public async Task<Guid> AddAsync(User user)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddRange(new[]
            {
            new SqlParameter("@Action", "INSERT"),
            new SqlParameter("@Id", user.Id),
            new SqlParameter("@Email", user.Email ?? (object)DBNull.Value),
            new SqlParameter("@Password", user.Password ?? (object)DBNull.Value),
            new SqlParameter("@UserName", user.UserName ?? (object)DBNull.Value),
            new SqlParameter("@AccessLevel", user.AccessLevel ?? (object)DBNull.Value),
            new SqlParameter("@CreatedDate", user.CreatedDate),
            new SqlParameter("@CreatedBy", user.CreatedBy),
            new SqlParameter("@IsActive", user.IsActive)
        });

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return user.Id;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddRange(new[]
            {
            new SqlParameter("@Action", "UPDATE"),
            new SqlParameter("@Id", user.Id),
            new SqlParameter("@Email", user.Email ?? (object)DBNull.Value),
            new SqlParameter("@UserName", user.UserName ?? (object)DBNull.Value),
            new SqlParameter("@AccessLevel", user.AccessLevel ?? (object)DBNull.Value),
            new SqlParameter("@IsActive", user.IsActive)
        });

            await connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            return rowsAffected > 0;
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
            var rowsAffected = await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add(new SqlParameter("@Action", "EXISTS"));
            command.Parameters.Add(new SqlParameter("@Id", id));

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            await connection.CloseAsync();

            return result != null && (int)result == 1;
        }

        private static User MapUserFromReader(SqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetGuid("Id"),
                Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                Password = reader.IsDBNull("Password") ? null : reader.GetString("Password"),
                UserName = reader.IsDBNull("UserName") ? null : reader.GetString("UserName"),
                AccessLevel = reader.IsDBNull("AccessLevel") ? null : reader.GetString("AccessLevel"),
                CreatedDate = reader.GetDateTime("CreatedDate"),
                CreatedBy = reader.GetGuid("CreatedBy"),
                IsActive = reader.GetBoolean("IsActive")
            };
        }

        public async Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add(new SqlParameter("@Action", SqlDbType.VarChar) { Value = "VALIDATE_CREDENTIALS" });
            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar) { Value = email ?? (object)DBNull.Value });
            command.Parameters.Add(new SqlParameter("@Password", SqlDbType.VarChar) { Value = password ?? (object)DBNull.Value });

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            await connection.CloseAsync(); // Explicit close (though using statement handles this automatically)

            return Convert.ToInt32(result) == 1;
        }

    }
}
