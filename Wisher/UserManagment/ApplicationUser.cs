﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Wisher.Models;

namespace Wisher.UserManagment
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public override string Email { get; set; }

        public int Age { get; set; }

        [Required]
        // converter enum
        [JsonConverter(typeof (StringEnumConverter))]
        public GenderEnum Gender { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager,
            string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }

        public ApplicationUser()
        {
              FavCategories = new List<string>();
        }
        //Data for AI
        public List<string> FavCategories { get; set; }
  }
}