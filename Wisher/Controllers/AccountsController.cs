﻿using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;
using Wisher.UserManagment;
using Wisher.UserManagment.Repository;

namespace Wisher.Controllers
{
    [Authorize]
    [RoutePrefix("api/accounts")]
    public class AccountsController : ApiController
    {
        private readonly IUserRepository _repository;
        public AccountsController() : this(new AuthRepository()) { }
        public AccountsController(IUserRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        [Route("auth")]
        public IHttpActionResult AuthConfirm()
        {
            return Ok();
        }
        [AllowAnonymous]
        [Route("create")]
        public async Task<IHttpActionResult> CreateUser(UserBindingModel createUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result = await _repository.RegisterUser(createUserModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }
            
            return Ok();
        }
        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
