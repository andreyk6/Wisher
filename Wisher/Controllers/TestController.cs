using System.Web.Http;
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
    }
}
