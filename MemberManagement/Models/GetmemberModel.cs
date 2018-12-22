using MemberManagement.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MemberManagement.Models
{
    public class GetMemberModel
    {
        public int totalItems { get; set; }
        public GetMember[] items { get; set; }
    }
    public class GetMember
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string position { get; set; }
        public RoleAccount role { get; set; }
        public System.DateTime updated { get; set; }
    }
    public class MemberFilterOption
    {
        [Required]
        public int startPage { get; set; }
        [Required]
        public int limitPage { get; set; }
        public string searchType { get; set; }
        public string searchText { get; set; }
    }
}