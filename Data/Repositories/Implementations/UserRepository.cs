using Core.Entities;
using Dapper;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        //
        public async Task<User?> GetByIdAsync(Guid id)
        {
            var sql = @"SELECT * FROM Users WHERE Id = @Id";
            return await _connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id.ToString() });
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            var sql = @"SELECT * FROM Users WHERE UserName = @UserName";
            return await _connection.QuerySingleOrDefaultAsync<User>(sql, new { UserName = userName });
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var sql = @"SELECT * FROM Users WHERE Email = @Email";
            return await _connection.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<User> AddAsync(User user)
        {
            var sql = @"
                INSERT INTO Users 
                    (UserName, FullName, Email, MobileNumber, Language, Culture, PasswordHash, CreatedAt, UpdatedAt)
                VALUES
                    (@UserName, @FullName, @Email, @MobileNumber, @Language, @Culture, @PasswordHash, @CreatedAt, @UpdatedAt)
                RETURNING Id;"; // SQLite auto-increment Id
            var id = await _connection.ExecuteScalarAsync<string>(sql, user);
            Guid outGuid = new Guid();
            Guid.TryParse(id, out outGuid);

            user.Id = outGuid;
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            var sql = @"
                UPDATE Users SET
                    UserName = @UserName,
                    FullName = @FullName,
                    Email = @Email,
                    MobileNumber = @MobileNumber,
                    Language = @Language,
                    Culture = @Culture,
                    PasswordHash = @PasswordHash,
                    UpdatedAt = @UpdatedAt,
                    IsActive = @IsActive
                WHERE Id = @Id";

            await _connection.ExecuteAsync(sql, new {   UserName = user.UserName, 
                                                        FullName = user.FullName, 
                                                        Email = user.Email, 
                                                        MobileNumber = user.MobileNumber,
                                                        Language = user.Language, 
                                                        Culture = user.Culture,
                                                        PasswordHash = user.PasswordHash, 
                                                        UpdatedAt = user.UpdatedAt, 
                                                        IsActive = user.IsActive,
                                                        Id = user.Id.ToString() });
        }

        public async Task DeleteAsync(Guid id)
        {
            var sql = @"
                UPDATE Users SET
                    Active = 0;
                WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id.ToString() });
        }
    }
}
