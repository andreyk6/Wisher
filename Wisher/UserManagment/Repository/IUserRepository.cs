using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Wisher.UserManagment.Repository
{
    public interface IUserRepository : IDisposable
    {
        Task<IdentityResult> RegisterUser(UserBindingModel userModel);
        Task<IdentityUser> FindUser(string userName, string password);
        Task<ApplicationUser> FindByEmailAsync(string email);
    }
}