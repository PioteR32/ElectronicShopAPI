using ElectronicShopAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShopAppTests.Support;

// Separate store per test; fresh contexts verify persisted state, not tracked objects.
internal sealed class TestDatabase
{
    private readonly DbContextOptions<DataContext> options = new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

    public DataContext Open() => new(options);
}
