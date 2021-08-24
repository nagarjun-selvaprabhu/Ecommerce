using Microsoft.EntityFrameworkCore;
//using Ecommerce.Model;

namespace OrderEndpoint.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

       // public DbSet<Products>  products { get; set; }
    }
}
