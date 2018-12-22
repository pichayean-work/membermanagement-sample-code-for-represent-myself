using MemberManagement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberManagement.Interfaces
{
    interface IAccesstokenService
    {
        string GenerateAccesstoeken(string email, int minute = 60);
        Members VerifyAccessToken(string accessToken);
    }
}
