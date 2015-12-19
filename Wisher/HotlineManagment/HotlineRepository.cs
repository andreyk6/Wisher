using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Wisher.Data;
using Wisher.Models;

namespace Wisher.HotlineManagment
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
            if (_applicationDbContext.EbayCategories.Count() == 0)
            {
                _applicationDbContext.EbayCategories.AddRange(data);
                _applicationDbContext.SaveChanges();
            }
        }

        public async Task<List<CategoryInfo>> GetCategories()
        {
            return await _applicationDbContext.EbayCategories.ToListAsync();
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
