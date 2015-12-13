using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public ApplicationUser()
        {
            CategoryInfo = new HashSet<CategoryInfo>();
        }
        [Required]
        public string Name { get; set; }
        [Required]
        public override string Email { get; set; }
        [Required]
        public int Age { get; set; }
        // converter enum
        [JsonConverter(typeof(StringEnumConverter))]
        public GenderEnum Gender { get; set; }
        public ICollection<CategoryInfo> CategoryInfo { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager,
            string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }
    }
}