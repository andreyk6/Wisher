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
using Wisher.UserManagment;
using Wisher.UserManagment.Repository;

namespace Wisher.Controllers
{
    public class WishController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly HotlineRepository _hotlineRepository;
        private static List<CategoryInfo> _categories;

        public WishController()
        {
            _hotlineRepository = new HotlineRepository();
            _dbContext = new ApplicationDbContext();
            _categories = _hotlineRepository.GetCategories().Result;
        }

        [HttpPost]
        public async Task<IHttpActionResult> MakeWish(WishRequestModel wishRequest)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == wishRequest.UserId);
            var categories = await _hotlineRepository.GetCategories();

            var tempUserCats = categories.Where(c => user.CatsToChose.Contains(c.Id)).ToList();
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
                    user.CatsToChose = result;
                    _dbContext.SaveChanges();
                }
            }

            int targetLevel = 0;
            if (tempUserCats.Count(c => c.Level == 1) < 4)
            {
                if (tempUserCats.Count(c => c.Level == 2) > 5)
                {
                    //Return 2nd level cats
                    targetLevel = 2;
                }
                else if (tempUserCats.Count(c => c.Level == 3) > 10)
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

            var tempUserCats = categories.Where(c => user.CatsToChose.Contains(c.Id)  && c.Level==3).ToList();
            List<HotlineProductModel> productModels = new List<HotlineProductModel>();
            foreach (var item in tempUserCats.Take(10))
                productModels.Add(HotlineProductManager.GetToProducts(item));

            productModels = productModels.Where(r => r != null).ToList();
            return Ok(productModels);
        }

        private CategoryInfo[] GetTwoRandomCategories(ICollection<CategoryInfo> categories, int level)
        {
            Random rnd = new Random();
            return categories.Where(c=>c.Level==level).OrderBy(user => rnd.Next()).Take(2).ToArray();
        }

        public async Task<IHttpActionResult> MakeWish2(WishRequestV2Model wishRequest)
        {
            #region [ Get user and category ]
            //Get current user
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == wishRequest.UserId);
            if (user == null)
                return BadRequest("User not found");

            //Get info about current category
            var category = _categories.FirstOrDefault(c => c.EbayCategoryIntValue == wishRequest.CategoryId);
            if (category == null)
                return BadRequest("Category not found");
            #endregion

            //If user dont like the category - remove it from list and return new random category
            if (!wishRequest.IsLiked)
            {
                RemoveRefferencedCatsFromUserList(wishRequest, user);
                //TODO: return cat from parrent nested cats
                return Ok(GetRandomCategoryFromList(user.CatsToChose));
            }
            else
            {
                CategoryInfo selectedCategory;
                switch (category.Level)
                {
                    //if Level == 1,2 then return item from nested cat
                    case 1:
                    case 2:
                        //Remove cat from queue
                        user.CatsToChose.Remove(category.EbayCategoryIntValue);
                        //Get next category
                        selectedCategory = GetRndNestedCategoryFromList(user.CatsToChose, category.EbayCategoryIntValue);
                        //If selected category doesn't has nested cats then return random parent nested cat
                        //                  -0-   - 1st level
                        //                 | | |  
                        //we are here    ->0 0 0  - 2nd level
                        //cat hasnt nested cat |  
                        //                     0  - 3rd level
                        if (selectedCategory == null)
                            return BadRequest("Category " + category.EbayCategoryId + " does not contains nested cats");
                        break;
                    //if Level == 3 then return item from nested groups of parrent group (current.Parrent.Nested())
                    case 3:
                        //Add category to user favList and remove from current list
                        user.SellectedCats.Add(category.EbayCategoryIntValue);
                        user.CatsToChose.Remove(category.EbayCategoryIntValue);
                        
                        selectedCategory = TryToGetRelatedCategory(user, category);
                        if (selectedCategory == null)
                            return BadRequest("Category " + category.EbayParrentIntValue + " does not contains nested cats");
                        break;
                    default:
                        return BadRequest("Category Level does not support");
                }
                return Ok(selectedCategory);
            }
        }

        /// <summary>
        /// Use for last level category
        /// Returns parrent nested cat or random cat from parrent family
        /// Or 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        private CategoryInfo TryToGetRelatedCategory(ApplicationUser user, CategoryInfo category)
        {
            CategoryInfo selectedCategory;
//Get next category
            selectedCategory = GetRndNestedCategoryFromList(user.CatsToChose, category.EbayParrentIntValue);

            //If parent category doesn't has nested cats then return random parrent of parent nested cat
            if (selectedCategory == null)
            {
                var parrent = _categories.FirstOrDefault(c => c.EbayCategoryIntValue == category.EbayParrentIntValue);
                selectedCategory = GetRndNestedCategoryFromList(user.CatsToChose,
                    parrent.EbayParrentIntValue);
            }

            //If parrent doesnt has any nested cat return random category
            selectedCategory = GetRandomCategoryFromList(user.CatsToChose);
            return selectedCategory;
        }

        private CategoryInfo GetRandomCategoryFromList(PersistableIntCollection favCats)
        {
            var userCats = _categories.Where(c => favCats.Contains(c.EbayCategoryIntValue)).ToList();
            CategoryInfo result;

            //Return ramdom category from first non empty layer
            for (int i = 1; i <= 3; i++)
            {
                result = GetRandomCat(userCats, i);
                if (result != null) return result;
            }

            return null;
        }

        private CategoryInfo GetRndNestedCategoryFromList(PersistableIntCollection favCats, int parrentId)
        {
            //get nested cats from queue
            var userCats = _categories.Where(c => favCats.Contains(c.EbayCategoryIntValue) && c.EbayParrentIntValue == parrentId).ToList();

            //if cat has nested cats return random nested cat
            if (userCats.Any())
            {
                return GetRandomCat(userCats);
            }
            //else return null
            return null;
        }

        private static CategoryInfo GetRandomCat(List<CategoryInfo> cats)
        {
            Random rnd = new Random();
            return cats.OrderBy(id => rnd.Next()).FirstOrDefault();
        }

        private static CategoryInfo GetRandomCat(List<CategoryInfo> userCats, int level)
        {
            Random rnd = new Random();

            var cats = userCats.Where(c => c.Level == level).ToList();

            return cats.OrderBy(id => rnd.Next()).FirstOrDefault();
        }

        private static CategoryInfo GetNextCategoryInList(List<CategoryInfo> cats, CategoryInfo current)
        {
            CategoryInfo result = GetNestedCategory(cats, current);
            if (result == null)
            {
                if (current.Level != 1)
                {
                    CategoryInfo parrent =
                        _categories.FirstOrDefault(c => c.EbayCategoryIntValue == current.EbayParrentIntValue);
                    result = GetNextCategoryInList(cats, parrent);
                }
                else
                {
                    result = GetRandomCat(cats);
                }
            }
            return result;
        }

        private static CategoryInfo GetNestedCategory(List<CategoryInfo> cats, CategoryInfo current)
        {
            return cats.FirstOrDefault(c => c.EbayParrentIntValue == current.EbayCategoryIntValue);
        }

        private void RemoveRefferencedCatsFromUserList(WishRequestV2Model wishRequest, ApplicationUser user)
        {
            //Get nested cats
            var nestedCatsId =
                _categories.Where(c => c.EbayParrentIntValue == wishRequest.CategoryId)
                    .Select(c => c.EbayCategoryIntValue)
                    .ToList();

            //Get nested cats of nested cats
            var nestedNestedCatsId =
                _categories.Where(c => nestedCatsId.Contains(c.EbayParrentIntValue))
                    .Select(c => c.EbayCategoryIntValue)
                    .ToList();

            //Groupe nested and nestet->nested cats to single list
            var catsIdToRemove = nestedCatsId.Concat(nestedNestedCatsId).ToList();

            //Remove nested cats from user list
            if (catsIdToRemove.Count() > 0)
            {
                foreach (int id in catsIdToRemove)
                {
                    user.CatsToChose.Remove(id);
                }
                _dbContext.SaveChanges();
            }
        }
    }
}
