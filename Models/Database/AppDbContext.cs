using Microsoft.EntityFrameworkCore;

namespace testtask_tplus.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
