using MemberManagement.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberManagement.Models
{
    public class MemberModel
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string position { get; set; }
        public RoleAccount role { get; set; }
        public System.DateTime created { get; set; }
        public System.DateTime updated { get; set; }
        [JsonIgnore]
        public string image_type { get; set; }
        [JsonIgnore]
        public byte[] image_byte { get; set; }
        public string image {
            get
            {
                if (image_byte != null && image_type != null)
                {
                    return $"{image_type},{Convert.ToBase64String(image_byte)}";
                }
                return null;
            }
        }
    }
}