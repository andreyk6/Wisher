using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wisher.Data;
using Wisher.Models;
using Wisher.UserManagment.Repository;

namespace Wisher.Controllers
{
    public class WishController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;
        private EbayDataRepository _ebayRepository;

        public WishController()
        {
            _ebayRepository = new EbayDataRepository();
        }

        [HttpPost]
        public IHttpActionResult MakeWish(WishRequestModel wishRequest)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Name == wishRequest.UserId);
            var categories = _ebayRepository.GetCategories();

            if (wishRequest.TrueCategoryId != -1)
            {
                var nestedCats =
                    categories.Where(c => c.EbayParrentCategoryId == wishRequest.FalseCategoryId)
                        .Select(c => c.EbayCategoryId);

                var updCategories = user.FavCategories
                    .Select(
                        my => my
                    )
                    .Where(my => my != wishRequest.FalseCategoryId &&
                                 nestedCats.Contains(my) == false)
                    .ToList();

                var result = new PersistableIntCollection();
                foreach (var updCategory in updCategories)
                {
                    result.Add(updCategory);
                }
                user.FavCategories = result;
                _dbContext.SaveChanges();
            }

            var topLevelCats = categories.Where(c => c.Level == 1).Select(c => c.EbayCategoryId);
            var userTopLevelCats = user.FavCategories.Where(my => topLevelCats.Contains(my)).ToList();

            int progressValue = (user.FavCategories.Count*100)/categories.Count;

            if (userTopLevelCats.Count < 7)
            {
                List<int> secondLevelCats = new List<int>();
                int index = 0;
                while (secondLevelCats.Count < 2 && index < user.FavCategories.Count)
                {
                    if (topLevelCats.Contains(user.FavCategories.ElementAt(index)) == false)
                    {
                        secondLevelCats.Add(user.FavCategories.ElementAt(index));
                    }
                }
                if (secondLevelCats.Count == 2)
                {
                    return Ok(new {catId1 = secondLevelCats[0], catId2 = secondLevelCats[1], progress = progressValue});
                }
                else
                {
                    return Ok(new {catId1 = -1, catId2 = -1, progress = 100});
                }
            }
            else
            {
                List<int> firstLevelCats = new List<int>();
                int index = 0;
                while (firstLevelCats.Count < 2 && index < user.FavCategories.Count)
                {
                    if (topLevelCats.Contains(user.FavCategories.ElementAt(index)))
                    {
                        firstLevelCats.Add(user.FavCategories.ElementAt(index));
                    }
                }
                return Ok(new {catId1 = firstLevelCats[0], catId2 = firstLevelCats[1], progress = progressValue});
            }
        }
    }
}
