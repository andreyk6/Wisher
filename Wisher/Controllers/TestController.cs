using System;
using System.Web.Http;
using Wisher.HotlineManagment;
using Wisher.Models;
using Wisher.UserManagment.Repository;

namespace Wisher.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : ApiController
    {
        [Route("update")]
        [HttpGet]
        public IHttpActionResult UpdateCategoriesFromEbay()
        {
            HotlineRepository repo = new HotlineRepository();
            repo.UpdateDbFromHotline();
            return Ok();
        }

        [Route("getTopItem")]
        [HttpGet]
        public IHttpActionResult GetRandomTopItem()
        {
            CategoryInfo category = new CategoryInfo() {Level = 2};

            HotlineRepository repo = new HotlineRepository();
            Random rnd = new Random();
            while (category.Level == 2)
            {
                category = repo.GetCategories()[rnd.Next(0, repo.GetCategories().Count)];
            }

            return Ok(HotlineProductManager.GetToProducts(category));
        }

    }
}
