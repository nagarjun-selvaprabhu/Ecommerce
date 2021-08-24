using Microsoft.EntityFrameworkCore;
using Ecommerce.Model;

namespace Ecommerce.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Products>  products { get; set; }
    }
}
