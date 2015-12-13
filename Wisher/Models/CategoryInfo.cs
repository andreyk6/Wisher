using System.Collections.Generic;
using Wisher.UserManagment;

namespace Wisher.Models
{
    public class CategoryInfo
    {
        public CategoryInfo()
        {
            LikedInfo = new HashSet<ApplicationUser>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string PictureUrl { get; set; }
        public int Level { get; set; }
        public int HotLIneCategoryId { get; set; }
        public int HotLineParrentCategoryId { get; set; }
        public ICollection<ApplicationUser> LikedInfo { get; set; } 
    }
}
