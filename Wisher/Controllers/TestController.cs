using System.Web.Http;

namespace Wisher.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : ApiController
    {
        [Route("update")]
        [HttpGet]
        public IHttpActionResult UpdateCategoriesFromEbay()
        {
            var result = HotlineCategoryManager.GetCategories();
            return Ok(result);
        }
    }
}
