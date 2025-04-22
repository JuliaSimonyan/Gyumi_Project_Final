using GyumriFinalVersion.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Gyumri.Data.Models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Place> Places { get; set; }
        //public DbSet<ApplicationUser> Users { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    }
}
