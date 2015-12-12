using System.Data.Entity;
using System.Web.DynamicData;
using Microsoft.AspNet.Identity.EntityFramework;
using Wisher.Models;
using Wisher.UserManagment;

namespace Wisher.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<CategoryInfo> EbayCategories { get; set; }

        public ApplicationDbContext() : base("wisher_db")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
    }
}