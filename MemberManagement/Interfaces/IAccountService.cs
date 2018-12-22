using MemberManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberManagement.Interfaces
{
    interface IAccountService
    {
        void Register(RegisterModel model);
        bool Login(LoginModel model);
    }
}
