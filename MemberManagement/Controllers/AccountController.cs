using MemberManagement.Interfaces;
using MemberManagement.Models;
using MemberManagement.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MemberManagement.Controllers
{
    public class AccountController : ApiController
    {
        private IAccountService Account;
        private IAccesstokenService Accesstoken;
        public AccountController()
        {
            this.Account = new AccountService();
            this.Accesstoken = new JWTAccessTokenService();
        }

        //register
        [Route("api/account/register")]
        public IHttpActionResult PostRegister([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                model.password = PasswordHashModel.Hash(model.password);
                try
                {
                    this.Account.Register(model);
                    return Ok("Successful");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Exception", ex.Message);
                }

            }
            return BadRequest(ModelState.GetErrorModelState());
        }

        [Route("api/account/login")]
        public AccessTokenModel PostLogin([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (this.Account.Login(model))
                    {
                        return new AccessTokenModel {
                            accessToken = this.Accesstoken.GenerateAccesstoeken(model.email)
                        };
                    }
                    throw new Exception("Username Or Password Is Invalid");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Exception", ex.Message);
                }
            }
            throw new HttpResponseException(Request.CreateResponse(
                HttpStatusCode.BadRequest,
                new { Message = ModelState.GetErrorModelState() }
            ));
        }
    }
}
