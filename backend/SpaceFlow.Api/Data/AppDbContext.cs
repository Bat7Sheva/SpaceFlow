using Microsoft.EntityFrameworkCore;

namespace SpaceFlow.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
}
