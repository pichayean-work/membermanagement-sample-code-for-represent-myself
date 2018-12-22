﻿using MemberManagement.Entity;
using MemberManagement.Interfaces;
using MemberManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberManagement.Service
{
    public class MemberService : IMemberService
    {
        private MacusYEntities db = new MacusYEntities();

        public IEnumerable<Members> MemberItems => this.db.Members.ToList();

        public void ChangePassword(string email, ChangePasswordModel model)
        {
            try
            {
                var memberItem = db.Members.SingleOrDefault(item => item.email.Equals(email));
                if (memberItem == null) throw new Exception("Not found member");
                if (!PasswordHashModel.Verify(model.old_pass, memberItem.password))
                {
                    throw new Exception("Old-Password worng");
                }
                else
                {
                    this.db.Members.Attach(memberItem);
                    memberItem.password = PasswordHashModel.Hash(model.new_pass);
                    memberItem.updated = DateTime.Now;
                    this.db.Entry(memberItem).State = System.Data.Entity.EntityState.Modified;
                    this.db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex.GetErrorException();
            }
            
        }
        public GetMemberModel GetMembers(MemberFilterOption filters)
        {
            if (!string.IsNullOrEmpty(filters.searchType) && filters.searchText.Equals("updated"))
            {
                var paramItem = HttpContext.Current.Request.Params;
                var fromDate = paramItem.Get("searchText[from]").Replace(" GMT+0700 (Indochina Time)", "");
                var toDate = paramItem.Get("searchText[to]").Replace(" GMT+0700 (Indochina Time)", "");
                filters.searchText = $"{fromDate},{toDate}";
            }
            var items = this.db.Members.Select(m => new GetMember
            {
                id = m.id,
                email = m.email,
                firstname = m.firstname,
                lastname = m.lastname,
                position = m.position,
                role = m.role,
                updated = m.updated
            })
            .OrderByDescending(m=>m.updated);
            var memberItems = new GetMemberModel
            {
                items = items
                        .Skip((filters.startPage - 1) * filters.limitPage)
                        .Take(filters.limitPage)
                        .ToArray(),
                totalItems = items.Count()
            };
            if (!string.IsNullOrEmpty(filters.searchType) && !string.IsNullOrEmpty(filters.searchText))
            {
                string searchText = filters.searchText;
                string searchType = filters.searchType;
                IEnumerable<GetMember> searchItem = new GetMember[] { };

                switch (searchType)
                {
                    case "updated":
                        var searchTexts = searchText.Split(',');
                        DateTime FromDate = DateTime.Parse(searchTexts[0]);
                        DateTime ToDate = DateTime.Parse(searchTexts[1]);
                        searchItem = from m in items
                                     where m.updated >= FromDate && m.updated <= ToDate
                                     select m;
                        break;
                    case "role":
                        searchItem = from m in items
                                    where Convert.ToInt16(m.GetType()
                                        .GetProperty(filters.searchType)
                                        .GetValue(m)) == Convert.ToInt16(searchText)
                                    select m;
                        break;
                    default:
                        searchItem = from m in items
                                     where m.GetType()
                                         .GetProperty(filters.searchType)
                                         .GetValue(m)
                                         .ToString()
                                         .ToLower()
                                         .Contains(searchText)
                                     select m;
                        break;
                }
                memberItems.items = searchItem
                        .Skip((filters.startPage - 1) * filters.limitPage)
                        .Take(filters.limitPage)
                        .ToArray();
            }
            return memberItems;
        }

        public void UpdateProfile(string email, ProfileModel model)
        {
            try
            {
                var memberItem = db.Members.SingleOrDefault(i => i.email.Equals(email));
                if (memberItem == null) throw new Exception("Not Found Member");
                this.db.Members.Attach(memberItem);
                memberItem.firstname = model.firstname;
                memberItem.lastname = model.lastname;
                memberItem.position = model.position;
                memberItem.updated = DateTime.Now;
                this.onConvertBase64ToImage(memberItem, model.image);
                //if (!String.IsNullOrEmpty(model.image))
                //{
                //    string[] images = model.image.Split(',');
                //    if (images.Length == 2)
                //    {
                //        if (images[0].IndexOf("image") >= 0)
                //        {
                //            memberItem.image = Convert.FromBase64String(images[1]);
                //            memberItem.image_type = images[0];
                //        }
                //    }
                //}
                //else {
                //    if (model.image == null)
                //    {
                //        memberItem.image = null;
                //        memberItem.image_type = null;
                //    }
                //}
                this.db.Entry(memberItem).State = System.Data.Entity.EntityState.Modified;
                this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex.GetErrorException();
            }
        }

        public void CreateMember(CreateMemberModel model)
        {
            try
            {
                Members memberCreate = new Members
                {
                    email = model.email,
                    password = PasswordHashModel.Hash(model.password),
                    firstname = model.password,
                    lastname = model.lastname,
                    position = model.position,
                    role = model.role,
                    created = DateTime.Now,
                    updated = DateTime.Now
                };

                this.onConvertBase64ToImage(memberCreate, model.image);
                this.db.Members.Add(memberCreate);
                this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex.GetErrorException();
            }
        }

        public void DeleteMember(int id)
        {
            try
            {
                var memberDelete = this.db.Members.SingleOrDefault(m => m.id == id);
                if (memberDelete == null) throw new Exception("Not found member");
                this.db.Members.Remove(memberDelete);
                this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex.GetErrorException();
            }
        }

        public void UpdateMember(int id, UpdateMemberModel model)
        {
            try
            {
                var memberUpdate = this.db.Members.SingleOrDefault(m => m.id == id);
                if (memberUpdate == null) throw new Exception("Member not found");
                this.db.Members.Attach(memberUpdate);
                memberUpdate.email = model.email;
                memberUpdate.firstname = model.firstname;
                memberUpdate.lastname = model.lastname;
                memberUpdate.position = model.position;
                memberUpdate.role = model.role;
                if (!string.IsNullOrEmpty(model.password))
                {
                    memberUpdate.password = model.password;
                }
                this.onConvertBase64ToImage(memberUpdate, model.image);
                memberUpdate.updated = DateTime.Now;
                this.db.Entry(memberUpdate).State = System.Data.Entity.EntityState.Modified;
                this.db.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex.GetErrorException();
            }
        }

        private void onConvertBase64ToImage(Members memberItem, string image)
        {

            if (!String.IsNullOrEmpty(image))
            {
                string[] images = image.Split(',');
                if (images.Length == 2)
                {
                    if (images[0].IndexOf("image") >= 0)
                    {
                        memberItem.image = Convert.FromBase64String(images[1]);
                        memberItem.image_type = images[0];
                    }
                }
            }
            else
            {
                if (image == null)
                {
                    memberItem.image = null;
                    memberItem.image_type = null;
                }
            }
        }
    }
}