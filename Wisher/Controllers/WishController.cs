using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Wisher.Data;
using Wisher.Models;
using Wisher.UserManagment.Repository;

namespace Wisher.Controllers
{
    public class WishController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly HotlineRepository _hotlineRepository;

        public WishController()
        {
            _hotlineRepository = new HotlineRepository();
            _dbContext = new ApplicationDbContext();
        }

        [HttpPost]
        public async Task<IHttpActionResult> MakeWish(WishRequestModel wishRequest)
        {
            var user = _dbContext.Users.Include(u => u.CategoryInfo).FirstOrDefault(u => u.Id == wishRequest.UserId);
            var categories = await _hotlineRepository.GetCategories();

            //Remove bad cats and nested cats from user wishlist
            if (wishRequest.FalseCategoryId != -1)
            {
                //var nestedCats = (from cats in categories
                //    where cats.EbayParrentIntValue == wishRequest.FalseCategoryId
                //    select cats.EbayCategoryId).ToList();
                var nestedCats = categories.Where(c => c.EbayParrentIntValue == wishRequest.FalseCategoryId).Select(c => c.EbayCategoryIntValue).ToList();

                //var nestedNestedCats = (from cats in categories
                //    where nestedCats.Contains(cats.EbayCategoryId)
                //    select cats.EbayCategoryId).ToList();
                var nestedNestedCats = categories.Where(c => nestedCats.Contains(c.EbayParrentIntValue)).Select(c => c.EbayCategoryIntValue).ToList();

                var catsToRemove = nestedCats.Concat(nestedNestedCats);

                if (catsToRemove.Count() > 0)
                {
                    var updCategories = user.CategoryInfo
                        .Select(
                            my => my
                        )
                        .Where(my => my.EbayCategoryIntValue != wishRequest.FalseCategoryId &&
                                     catsToRemove.Contains(my.EbayCategoryIntValue) == false)
                        .ToList();

                    user.CategoryInfo = updCategories;
                    _dbContext.SaveChanges();
                }
            }

            int targetLevel = 0;
            if (user.CategoryInfo.Count(c => c.Level == 1) < 4)
            {
                if (user.CategoryInfo.Count(c => c.Level == 2) > 5)
                {
                    //Return 2nd level cats
                    targetLevel = 2;
                }
                else if (user.CategoryInfo.Count(c => c.Level == 3) > 10)
                {
                    //return 3rd level cats
                    targetLevel = 3;
                }
                else
                {
                    //return 100% result
                    return Ok(new { cat1_id = "", cat2_id = "", progress = 100 });
                }
            }
            else
            {
                //return 1level cats
                targetLevel = 1;
            }

            var rndCats = GetTwoRandomCategories(user.CategoryInfo, targetLevel);
            return Ok(new
            {
                cat1_id = rndCats[0].EbayCategoryIntValue,
                cat1_name = rndCats[0].Name,
                cat2_id = rndCats[1].EbayCategoryIntValue,
                cat2_name = rndCats[1].Name,
                progress = 100 - ((user.CategoryInfo.Count * 100) / categories.Count)
            });
        }

        private CategoryInfo[] GetTwoRandomCategories(ICollection<CategoryInfo> categories, int level)
        {
            Random rnd = new Random();
            return categories.OrderBy(user => rnd.Next()).Take(2).ToArray();
        }
    }
}
