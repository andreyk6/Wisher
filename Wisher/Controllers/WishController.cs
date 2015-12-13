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
        //private EbayDataRepository _ebayRepository;

        public WishController()
        {
           // _ebayRepository = new EbayDataRepository();
            _dbContext = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult MakeWish(WishRequestModel wishRequest)
        {
            return null;
        }
    }
}
