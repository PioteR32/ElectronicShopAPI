using ElectronicShopAPI.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShopAppTests.Support;

internal sealed class OrderDatabase : IDisposable
{
    private readonly SqliteConnection connection = new("Data Source=:memory:");
    public OrderDatabase()
    {
        connection.Open();
        using var db = Open();
        db.Database.EnsureCreated();
    }
    public DataContext Open() => new(new DbContextOptionsBuilder<DataContext>().UseSqlite(connection).Options);
    public void Dispose() => connection.Dispose();
}
