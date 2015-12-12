using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisher.Models
{
    public class CategoryInfo
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public string PictureUrl { get; set; }
        public int Level { get; set; }

        public int EbayCategoryId { get; set; }
        public int EbayParrentCategoryId { get; set; }

        public int Id { get; set; }
    }
}
