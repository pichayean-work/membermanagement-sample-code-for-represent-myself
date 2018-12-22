using MemberManagement.Entity;
using MemberManagement.Interfaces;
using MemberManagement.Models;
using MemberManagement.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


// asp9/6
namespace MemberManagement.Controllers
{
    [Authorize]
    public class MemberController : ApiController
    {
        private IMemberService memberService;

        public MemberController()
        {
            this.memberService = new MemberService();
        }

        [Route("api/member/data")]
        public MemberModel GetMemberLogin()
        {
            var userLogin = this.memberService.MemberItems.SingleOrDefault(item => item.email.Equals(User.Identity.Name));
            if (userLogin == null) return null;
            return new MemberModel
            {
                id = userLogin.id,
                created = userLogin.created,
                email = userLogin.email,
                firstname = userLogin.firstname,
                image_byte = userLogin.image,
                image_type = userLogin.image_type,
                lastname = userLogin.lastname,
                position = userLogin.position,
                role = userLogin.role,
                updated = userLogin.updated
            };
        }

        [Route("api/member/profile")]
        public IHttpActionResult PostUpdateProfile([FromBody] ProfileModel model) {
            if (ModelState.IsValid)
            {
                try
                {
                    this.memberService.UpdateProfile(User.Identity.Name, model);
                    return Ok(this.GetMemberLogin());
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("exeception", ex.Message);
                }
            }
            return BadRequest(ModelState.GetErrorModelState());
        }

        [Route("api/member/change-password")]
        public IHttpActionResult PostChangPassword([FromBody] ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.memberService.ChangePassword(User.Identity.Name, model);
                    return Ok(new { message = "Password has Change"});
                }
                catch (Exception ex)
                {

                    throw ex.GetErrorException() ;
                }
            }
            return Json(ModelState.GetErrorModelState());
        }

        [Authorize(Roles = "Admin")]
        public IHttpActionResult PostCreateMember([FromBody] CreateMemberModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.memberService.CreateMember(model);
                    return Ok("Create successful");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Exception", ex.Message);
                }
            }
            return BadRequest(ModelState.GetErrorModelState());
        }

        [Authorize(Roles = "Admin")]
        public MemberModel GetMember(int id) {
            return this.memberService.MemberItems
                .Select(m => new MemberModel
                {
                    id = m.id,
                    created = m.created,
                    email = m.email,
                    firstname = m.firstname,
                    image_byte = m.image,
                    image_type = m.image_type,
                    lastname = m.lastname,
                    position = m.position,
                    role = m.role,
                    updated = m.updated
                })
                .SingleOrDefault(m => m.id == id);
        }

        [Authorize(Roles = "Employee,Admin")]
        public GetMemberModel GetMembers([FromUri] MemberFilterOption filters)
        {
            if (ModelState.IsValid)
            {
                return this.memberService.GetMembers(filters);
            }
            throw new HttpResponseException(Request.CreateResponse(
                HttpStatusCode.BadRequest,
                new { Message = ModelState.GetErrorModelState() }
            ));
        }

        [Authorize(Roles = "Employee,Admin")]
        public IHttpActionResult DeleteMember(int id) {
            try
            {
                this.memberService.DeleteMember(id);
                return Ok("Delete Succesful");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);
            }
            return BadRequest(ModelState.GetErrorModelState());
        }

        [Authorize(Roles = "Admin")]
        public IHttpActionResult PutUpdateMember(int id, [FromBody] UpdateMemberModel model) {
            if (ModelState.IsValid)
            {
                try
                {
                    this.memberService.UpdateMember(id, model);
                    return Ok("Update successful");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("exception", ex.Message);
                }
            }
            return BadRequest(ModelState.GetErrorModelState());
        }
    }
}
