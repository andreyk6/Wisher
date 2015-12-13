using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Routing;
using Wisher.Data;
using Wisher.HotlineManagment;
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
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == wishRequest.UserId);
            var categories = await _hotlineRepository.GetCategories();

            var tempUserCats = categories.Where(c => user.FavCats.Contains(c.Id)).ToList();
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

                var catsToRemove = nestedCats.Concat(nestedNestedCats).ToList();

                if (catsToRemove.Count() > 0)
                {
                    tempUserCats = tempUserCats
                        .Select(
                            my => my
                        )
                        .Where(my => my.EbayCategoryIntValue != wishRequest.FalseCategoryId &&
                                     catsToRemove.Contains(my.EbayCategoryIntValue) == false)
                        .ToList();

                    var result = new PersistableIntCollection();
                    foreach (var categoryInfo in tempUserCats)
                    {
                        result.Add(categoryInfo.EbayCategoryIntValue);
                    }
                    user.FavCats = result;
                    _dbContext.SaveChanges();
                }
            }

            int targetLevel = 0;
            if (tempUserCats.Count(c => c.Level == 1) < 5)
            {
                if (tempUserCats.Count(c => c.Level == 2) > 3)
                {
                    //Return 2nd level cats
                    targetLevel = 2;
                }
                else if (tempUserCats.Count(c => c.Level == 3) > 25)
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

            var rndCats = GetTwoRandomCategories(tempUserCats.ToList(), targetLevel);
            return Ok(new
            {
                cat1_id = rndCats[0].EbayCategoryIntValue,
                cat1_name = rndCats[0].Name,
                cat2_id = rndCats[1].EbayCategoryIntValue,
                cat2_name = rndCats[1].Name,
                progress = 112 - ((tempUserCats.Count() * 100) / categories.Count)
            });
        }

        [Route("api/wish/getTop/{userName}")]
        public async Task<IHttpActionResult> GetTopProduct(string userName)
        {
            var user = _dbContext.Users.FirstOrDefault(r => r.Id == userName);
            if (user == null) return BadRequest();
            var categories = await _hotlineRepository.GetCategories();

            var tempUserCats = categories.Where(c => user.FavCats.Contains(c.Id)  && c.Level!=2).ToList();
            List<HotlineProductModel> productModels = new List<HotlineProductModel>();
            foreach (var item in tempUserCats.Take(15))
                productModels.Add(HotlineProductManager.GetToProducts(item));

            productModels = productModels.Where(r => r != null).ToList();
            return Ok(productModels);
        }

        private CategoryInfo[] GetTwoRandomCategories(ICollection<CategoryInfo> categories, int level)
        {
            Random rnd = new Random();
            return categories.Where(c=>c.Level==level).OrderBy(user => rnd.Next()).Take(2).ToArray();
        }
    }
}
