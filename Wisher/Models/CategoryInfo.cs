using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wisher.UserManagment;

namespace Wisher.Models
{
    public class CategoryInfo
    {
        public CategoryInfo()
        {
            LikedInfo = new HashSet<ApplicationUser>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string PictureUrl { get; set; }
        public int Level { get; set; }
        public ICollection<ApplicationUser> LikedInfo { get; set; } 
        public string EbayCategoryId { get; set; }
        public int EbayCategoryIntValue { get; set; }
        public string EbayParrentCategoryId { get; set; }
        public int EbayParrentIntValue { get; set; }
    }
}
