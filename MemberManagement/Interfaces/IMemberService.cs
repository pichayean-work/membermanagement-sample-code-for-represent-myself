using MemberManagement.Entity;
using MemberManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberManagement.Interfaces
{
    interface IMemberService
    {
        IEnumerable<Members> MemberItems { get; }
        void UpdateProfile(string email, ProfileModel model);
        void ChangePassword(string email, ChangePasswordModel model);
        GetMemberModel GetMembers(MemberFilterOption filters);
        void CreateMember(CreateMemberModel model);
        void DeleteMember(int id);
        void UpdateMember(int id, UpdateMemberModel model);
    }
}
