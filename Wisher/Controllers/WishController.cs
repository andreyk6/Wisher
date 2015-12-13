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
        public async  Task<IHttpActionResult> MakeWish(WishRequestModel wishRequest)
        {
            var user = await _dbContext.Users.Include(u => u.CategoryInfo).FirstOrDefaultAsync(u => u.Id == wishRequest.UserId);
            var categories = _hotlineRepository.GetCategories();

            //Remove bad cats and nested cats from user wishlist
            if (wishRequest.FalseCategoryId != "-1")
            {
                var nestedCats = categories.Where(c => c.EbayParrentCategoryId == wishRequest.FalseCategoryId)
                        .Select(c => c.EbayCategoryId);
                var nestedNestedCats =
                    categories.Where(c => nestedCats.Contains(c.EbayParrentCategoryId)).Select(c => c.EbayCategoryId);

                var catsToRemove = nestedCats.Concat(nestedNestedCats);

                var updCategories = user.CategoryInfo
                    .Select(
                        my => my
                    )
                    .Where(my => my.EbayCategoryId != wishRequest.FalseCategoryId &&
                                 catsToRemove.Contains(my.EbayCategoryId) == false)
                    .ToList();

                user.CategoryInfo = updCategories;
               await  _dbContext.SaveChangesAsync();
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
                cat1_id = rndCats[0].EbayCategoryId,
                cat1_name = rndCats[0].Name,
                cat2_id = rndCats[1].EbayCategoryId,
                cat2_name = rndCats[1].EbayCategoryId,
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
