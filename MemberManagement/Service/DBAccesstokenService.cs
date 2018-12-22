using MemberManagement.Entity;
using MemberManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MemberManagement.Models;
namespace MemberManagement.Service
{
    public class DBAccesstokenService: IAccesstokenService
    {
        private MacusYEntities db = new MacusYEntities();
        public string GenerateAccesstoeken(string email, int minute = 60)
        {
            try
            {
                var member = db.Members.SingleOrDefault(m => m.email.Equals(email));
                if (member == null) throw new Exception("not found member");

                var accessToken = new AccessTokens
                {
                    token = Guid.NewGuid().ToString(),
                    exprise = DateTime.Now.AddMinutes(minute),
                    memberID = member.id
                };
                this.db.AccessTokens.Add(accessToken);
                this.db.SaveChanges();
                return accessToken.token;
            }
            catch (Exception ex)
            {
                throw ex.GetErrorException();
            }
        }

        public Members VerifyAccessToken(string accessToken)
        {
            try
            {
                var accessTokenItem = this.db.AccessTokens.SingleOrDefault(a => a.token.Equals(accessToken));
                if (accessTokenItem == null) return null;
                if (accessTokenItem.exprise < DateTime.Now) return null;
                return accessTokenItem.Members;
            }
            catch
            {
                return null;
            }
        }
    }
}