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
        public int TrueCategoryId { get; set; }
        public int FalseCategoryId { get; set; }
    }
}
