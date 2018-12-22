using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MemberManagement.Interfaces;
using MemberManagement.Service;
using System.Security.Principal;
using MemberManagement.Entity;

namespace MemberManagement
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private IAccesstokenService accessTokenService;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorization = request.Headers.Authorization;

            if (authorization != null)
            {
                string AccessToken = authorization.Parameter;
                string AccessTokenType = authorization.Scheme;
                if (AccessTokenType.Equals("Bearer"))
                {
                    this.accessTokenService = new JWTAccessTokenService();
                    var memberItem = this.accessTokenService.VerifyAccessToken(AccessToken);
                    if (memberItem != null)
                    {
                        var UserLogin = new UserLogin(new GenericIdentity(memberItem.email), memberItem.role);
                        UserLogin.member = memberItem;
                        Thread.CurrentPrincipal = UserLogin;
                        HttpContext.Current.User = UserLogin;
                    }
                }
            }
            return base.SendAsync(request, cancellationToken);
        }
    }

    class UserLogin : GenericPrincipal
    {
        public Members member { get; set; }
        public UserLogin(IIdentity identity, RoleAccount roles) : base(identity, new string[] { roles.ToString()})
        {
        }
    }
}