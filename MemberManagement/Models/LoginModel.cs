using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MemberManagement.Models
{
	public class LoginModel
	{
		[Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string remember { get; set; }
    }
    public class AccessTokenModel
    {
        public string accessToken { get; set; }
    }
}