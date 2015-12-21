using System.Data.Entity;
using System.Web.DynamicData;
using Microsoft.AspNet.Identity.EntityFramework;
using Wisher.Models;
using Wisher.UserManagment;
using Wisher.UserManagment.Models;

namespace Wisher.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<CategoryInfo> EbayCategories { get; set; }

        public ApplicationDbContext() : base("wisher_db", throwIfV1Schema:false)
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}