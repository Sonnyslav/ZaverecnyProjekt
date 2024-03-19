using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InsuranceCompany.Models;

namespace InsuranceCompany.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<InsuranceCompany.Models.Client> Client { get; set; } = default!;
        public DbSet<InsuranceCompany.Models.Insurance> Insurance { get; set; } = default!;
        public DbSet<InsuranceCompany.Models.InsuredEvent> InsuredEvent { get; set; } = default!;
    }
}
