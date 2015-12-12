using Microsoft.AspNet.Identity.EntityFramework;
using Wisher.UserManagment;

namespace Wisher.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("LocalDbUsers")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
    }
}