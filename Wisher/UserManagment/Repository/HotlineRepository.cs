using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Wisher.Data;
using Wisher.Models;

namespace Wisher.UserManagment.Repository
{
    public class HotlineRepository
    {
        private bool _disposed;
        private readonly ApplicationDbContext _applicationDbContext;
        public HotlineRepository()
        {
            _applicationDbContext = new ApplicationDbContext();
        }

        public void UpdateDbFromHotline()
        {
            var data = HotlineCategoryManager.GetCategories();
            _applicationDbContext.EbayCategories.AddRange(data);
            _applicationDbContext.SaveChanges();
        }

        public List<CategoryInfo> GetCategories()
        {
            return _applicationDbContext.EbayCategories.ToList();
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
