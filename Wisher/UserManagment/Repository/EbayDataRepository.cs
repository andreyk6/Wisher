﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Wisher.Data;
using Wisher.EbayManagement;

namespace Wisher.UserManagment.Repository
{
    public class EbayDataRepository:IDisposable
    {
        private bool _disposed;
        private readonly ApplicationDbContext _applicationDbContext;

        public EbayDataRepository()
        {
            _applicationDbContext = new ApplicationDbContext();
        }

        public void UpdateCategoriesFromEBay()
        {
            //For future use only!!!!
            var categories = EbayCategoriesManager.GetCategories();
            categories = categories.Select(c => c).Where(c => c.Level <= 2).ToList();

            _applicationDbContext.EbayCategories.AddRange(categories);

            _applicationDbContext.SaveChanges();
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
