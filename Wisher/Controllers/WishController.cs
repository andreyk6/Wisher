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
        private List<CategoryInfo> _userCats; 

        public WishController()
        {
            _hotlineRepository = new HotlineRepository();
            _dbContext = new ApplicationDbContext();
            _categories = _hotlineRepository.GetCategories();
        }

        [Route("api/wish/getTop/{userName}")]
        public async Task<IHttpActionResult> GetTopProduct(string userName)
        {
            var user = _dbContext.Users.FirstOrDefault(r => r.Id == userName);
            if (user == null) return BadRequest();
            var categories = await _hotlineRepository.GetCategoriesAsync();

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

        [HttpPost]
        public async Task<IHttpActionResult> DoWish(WishRequestV2Model wishRequest)
        {
            #region [ Get user and category ]
            //Get current user
            var user = _dbContext.Users.FirstOrDefault(u => u.UserName == wishRequest.UserId);
            if (user == null)
                return BadRequest("User not found");

            if (_userCats == null)
            {
                _userCats = _categories.Where(c => user.CatsToChose.Contains(c.EbayCategoryIntValue)).ToList();
            }
            #endregion

            CategoryInfo next;
            if (wishRequest.CategoryId == -1)
            {
                if (user.LastCatStoreId != 0)
                    next = _categories.FirstOrDefault(c => c.EbayCategoryIntValue == user.LastCatStoreId);
                next = GetRandomCat(_userCats);
            }
            else
            {
                //Get info about current category
                var category = _categories.FirstOrDefault(c => c.EbayCategoryIntValue == wishRequest.CategoryId);
                if (category == null)
                    return BadRequest("Category not found");

                //If user dont like the category - remove it from list and return new random category
                if (!wishRequest.IsLiked)
                {
                    //Remove all refferenced categories
                    RemoveRefferencedCatsFromUserList(category, user);
                    next = GetRandomCat(_userCats);
                }
                else
                {
                    //If it is last cat level then add it to favourite cats
                    if (category.Level == 3)
                        user.SellectedCats.Add(category.EbayCategoryIntValue);

                    //Remove from current queue
                    _userCats.Remove(category);
                    user.CatsToChose.Remove(category.EbayCategoryIntValue);

                    _dbContext.SaveChanges();

                    next = GetNextCategoryInList(_userCats, category);
                }

                if (next == null)
                    return Ok(new
                    {
                        progress = 100
                    });

                user.LastCatStoreId = next.EbayCategoryIntValue;
                _dbContext.SaveChanges();
            }
            return Ok(new
            {
                name = next.Name,
                categoryId = next.EbayCategoryIntValue,
                progress = 100 - (user.CatsToChose.Count*100)/_categories.Count
            });

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

        private static CategoryInfo GetRandomCat(List<CategoryInfo> userCats)
        {
            Random rnd = new Random();
            for (int level = 1; level <= 3; level++)
            {
                var cat = userCats
                    .Where(c => c.Level == level)
                    .OrderBy(id => rnd.Next())
                    .FirstOrDefault();

                if (cat != null) return cat;
            }
            return null;
        }

        private static CategoryInfo GetNestedCategory(List<CategoryInfo> cats, CategoryInfo current)
        {
            return cats.FirstOrDefault(c => c.EbayParrentIntValue == current.EbayCategoryIntValue);
        }

        private void RemoveRefferencedCatsFromUserList(CategoryInfo category, ApplicationUser user)
        {
            //Get nested cats
            var catsToRemove =
                _categories.Where(c => c.EbayParrentIntValue == category.EbayCategoryIntValue)
                    .Select(c => c.EbayCategoryIntValue)
                    .ToList();

            //Get nested cats of nested cats
            var nestedNestedCatsId =
                _categories.Where(c => catsToRemove.Contains(c.EbayParrentIntValue))
                    .Select(c => c.EbayCategoryIntValue)
                    .ToList();

            //Groupe nested and nestet->nested cats to single list
            catsToRemove.AddRange(nestedNestedCatsId);

            //Add current Id to remove list
            catsToRemove.Add(category.EbayCategoryIntValue);

            //Remove nested cats from user list
            if (catsToRemove.Count() > 0)
            {
                foreach (int id in catsToRemove)
                {
                    user.CatsToChose.Remove(id);
                }
                _dbContext.SaveChanges();
            }
        }
    }
}
