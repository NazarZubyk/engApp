using Back.Infrastructure.Persistence.Entities;
using Npgsql;

namespace Back.Infrastructure.Persistence;

public class AdminRepository : IAdminRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public AdminRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<AdminRecord?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(
            """
            SELECT id, username, password_hash
            FROM admins
            WHERE username = @username
            """,
            connection);

        command.Parameters.AddWithValue("username", username);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return new AdminRecord(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2));
    }
}
