using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisher.Models
{
    public class WishRequestModel
    {
        public string UserId { get; set; }
        public string TrueCategoryId { get; set; }
        public string FalseCategoryId { get; set; }
    }
}
