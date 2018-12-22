using MemberManagement.Entity;
using MemberManagement.Interfaces;
using MemberManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberManagement.Service
{
    public class AccountService : IAccountService
    {
        private MacusYEntities db = new MacusYEntities();
        public bool Login(LoginModel model)
        {
            try
            {
                var member = db.Members.SingleOrDefault(m => m.email == model.email);
                if (member != null)
                {
                    return PasswordHashModel.Verify(model.password, member.password);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
        public void Register(RegisterModel model)
        {
            try
            {
                this.db.Members.Add(new Members {
                    firstname = model.firstname,
                    lastname = model.lastname,
                    email = model.email,
                    password = model.password,
                    position = "",
                    image = null,
                    role = RoleAccount.Member,
                    created = DateTime.Now,
                    updated = DateTime.Now
                });
                this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex.GetErrorException();
            }
        }
    }
}