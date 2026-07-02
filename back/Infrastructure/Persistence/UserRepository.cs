using Back.Infrastructure.Persistence.Entities;
using Npgsql;

namespace Back.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public UserRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<IReadOnlyList<UserRecord>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(
            """
            SELECT id, login, password_hash, created_at
            FROM users
            ORDER BY id
            """,
            connection);

        var users = new List<UserRecord>();
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            users.Add(ReadUser(reader));
        }

        return users;
    }

    public async Task<UserRecord?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(
            """
            SELECT id, login, password_hash, created_at
            FROM users
            WHERE id = @id
            """,
            connection);

        command.Parameters.AddWithValue("id", id);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? ReadUser(reader) : null;
    }

    public async Task<UserRecord?> GetByLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(
            """
            SELECT id, login, password_hash, created_at
            FROM users
            WHERE login = @login
            """,
            connection);

        command.Parameters.AddWithValue("login", login);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? ReadUser(reader) : null;
    }

    public async Task<UserRecord> CreateAsync(string login, string passwordHash, CancellationToken cancellationToken = default)
    {
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(
            """
            INSERT INTO users (login, password_hash)
            VALUES (@login, @password_hash)
            RETURNING id, login, password_hash, created_at
            """,
            connection);

        command.Parameters.AddWithValue("login", login);
        command.Parameters.AddWithValue("password_hash", passwordHash);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            throw new InvalidOperationException("Failed to create user.");
        }

        return ReadUser(reader);
    }

    public async Task<UserRecord?> UpdateAsync(int id, string? login, string? passwordHash, CancellationToken cancellationToken = default)
    {
        if (login is null && passwordHash is null)
        {
            return await GetByIdAsync(id, cancellationToken);
        }

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(
            """
            UPDATE users
            SET
                login = COALESCE(@login, login),
                password_hash = COALESCE(@password_hash, password_hash)
            WHERE id = @id
            RETURNING id, login, password_hash, created_at
            """,
            connection);

        command.Parameters.AddWithValue("id", id);
        command.Parameters.AddWithValue("login", (object?)login ?? DBNull.Value);
        command.Parameters.AddWithValue("password_hash", (object?)passwordHash ?? DBNull.Value);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? ReadUser(reader) : null;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(
            "DELETE FROM users WHERE id = @id",
            connection);

        command.Parameters.AddWithValue("id", id);
        var rows = await command.ExecuteNonQueryAsync(cancellationToken);
        return rows > 0;
    }

    private static UserRecord ReadUser(NpgsqlDataReader reader) =>
        new(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetFieldValue<DateTimeOffset>(3));
}
