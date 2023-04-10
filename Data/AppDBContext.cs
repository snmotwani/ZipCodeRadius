using Microsoft.EntityFrameworkCore;
using ZipCodeRadius.Model;

namespace ZipCodeRadius.Data;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base (options)
    {}

    public DbSet<ZipCodeData> zipCodeDatas {get; set;}

    public DbSet<Blood> BloodTypes {get; set;}
}
