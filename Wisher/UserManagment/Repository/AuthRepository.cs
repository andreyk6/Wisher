using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Wisher.Data;

namespace Wisher.UserManagment.Repository
{
    public class AuthRepository : IUserRepository
    {
       
        private bool _disposed;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        public AuthRepository()
        {
            _applicationDbContext = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_applicationDbContext));
        }

        public async Task<IdentityResult> RegisterUser(UserBindingModel userModel)
        {
            var user = new ApplicationUser()
            {
                Name = userModel.Name,
                Email = userModel.Email,
                UserName = userModel.Name
            };

            var ebayRepo = new EbayDataRepository();
            var categoriesCodes = ebayRepo.GetCategoriesId();

            foreach (int categoriesCode in categoriesCodes)
            {
              //  user.FavCategories.Add(categoriesCode);
            }
            
            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    _applicationDbContext.Dispose();
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}