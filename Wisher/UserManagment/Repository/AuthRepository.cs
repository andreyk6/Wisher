using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Wisher.Data;
using Wisher.HotlineManagment;
using Wisher.Models;
using Wisher.UserManagment.Models;

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
            var categories = await new HotlineRepository().GetCategories();
            var res = new PersistableIntCollection();
            foreach (var categoryInfo in categories)
            {
                res.Add(categoryInfo.EbayCategoryIntValue);
            }

            var user = new ApplicationUser()
            {
                Name = userModel.Name,
                Email = userModel.Email,
                UserName = userModel.Email,
                Age = userModel.Age,
                Gender = userModel.Gender,
                FavCats = res
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }
        public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            IdentityUser user = await _userManager.FindAsync(loginInfo);

            return user;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser userModel)
        {
            var categories = await new HotlineRepository().GetCategories();
            var res = new PersistableIntCollection();
            foreach (var categoryInfo in categories)
            {
                res.Add(categoryInfo.EbayCategoryIntValue);
            }
            userModel.FavCats = res;

            var result = await _userManager.CreateAsync(userModel);

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);

            return result;
        }
        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }
        public Client FindClient(string clientId)
        {
            var client = _applicationDbContext.Clients.Find(clientId);

            return client;
        }
        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken = _applicationDbContext.RefreshTokens.SingleOrDefault(r => r.Subject == token.Subject && r.ClientId == token.ClientId);

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _applicationDbContext.RefreshTokens.Add(token);

            return await _applicationDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _applicationDbContext.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _applicationDbContext.RefreshTokens.Remove(refreshToken);
                return await _applicationDbContext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _applicationDbContext.RefreshTokens.Remove(refreshToken);
            return await _applicationDbContext.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _applicationDbContext.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _applicationDbContext.RefreshTokens.ToList();
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