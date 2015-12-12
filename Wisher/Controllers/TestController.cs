using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            EbayDataRepository repository = new EbayDataRepository();

            repository.UpdateCategoriesFromEBay();
            
            return Ok();
        }
    }
}
