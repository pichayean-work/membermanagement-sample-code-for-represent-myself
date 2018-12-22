using MemberManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jose;
using System.Text;
using MemberManagement.Entity;

namespace MemberManagement.Service
{
    public class JWTAccessTokenService : IAccesstokenService
    {
        private byte[] secretKey = Encoding.UTF8.GetBytes("I LOVE U");
        private MacusYEntities db = new MacusYEntities();

        public string GenerateAccesstoeken(string email, int minute = 60)
        {
            JWTPayload payload = new JWTPayload
            {
                email = email,
                exp = DateTime.Now.AddMinutes(minute)
            };

            return JWT.Encode(payload, this.secretKey, JwsAlgorithm.HS256);
        }

        public Members VerifyAccessToken(string accessToken)
        {
            try
            {
                JWTPayload payload = JWT.Decode<JWTPayload>(accessToken, this.secretKey);
                if (payload == null)  return null;
                if (payload.exp < DateTime.UtcNow) return null;
                return this.db.Members.SingleOrDefault(i => i.email == payload.email);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
    class JWTPayload
    {
        public string email { get; set; }
        public DateTime exp { get; set; }
    }
}