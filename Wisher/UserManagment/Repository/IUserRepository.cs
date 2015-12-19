using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Wisher.UserManagment.Models;

namespace Wisher.UserManagment.Repository
{
    public interface IUserRepository : IDisposable
    {
        Task<IdentityResult> RegisterUser(UserBindingModel userModel);
        Task<IdentityUser> FindUser(string userName, string password);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<IdentityUser> FindAsync(UserLoginInfo loginInfo);
        Task<IdentityResult> CreateAsync(ApplicationUser user);
        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);
        Client FindClient(string clientId);
    }
}