using SQLite;
using PasswordManager.Models;

namespace PasswordManager.Services;

public class LocalStorageService
{
    private SQLiteAsyncConnection _database;
    public const SQLite.SQLiteOpenFlags Flags =
    // open the database in read/write mode
    SQLite.SQLiteOpenFlags.ReadWrite |
    // create the database if it doesn't exist
    SQLite.SQLiteOpenFlags.Create |
    // enable multi-threaded database access
    SQLite.SQLiteOpenFlags.SharedCache;

    public LocalStorageService(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath, Flags);
        _database.CreateTableAsync<Credential>().Wait();
        _database.EnableWriteAheadLoggingAsync().Wait();
    }

    public Task<List<Credential>> GetCredentialsAsync()
    {
        return _database.Table<Credential>().ToListAsync();
    }

    public Task<Credential> GetCredentialAsync(Guid id)
    {
        return _database.Table<Credential>().Where(c => c.id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveCredentialAsync(Credential credential)
    {
        if (credential.id == Guid.Empty)
        {
            credential.id = Guid.NewGuid();
        }

        var existingCredential = await GetCredentialAsync(credential.id);
        if (existingCredential != null)
        {
            return await _database.UpdateAsync(credential);
        }
        else
        {
            return await _database.InsertAsync(credential);
        }
    }

    public Task<int> DeleteCredentialAsync(Credential credential)
    {
        return _database.DeleteAsync(credential);
    }

    public Task<int> DeleteAllCredentialsAsync()
    {
        return _database.DeleteAllAsync<Credential>();
    }
}