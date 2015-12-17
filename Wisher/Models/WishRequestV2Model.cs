using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisher.Models
{
    public class WishRequestV2Model
    {
        public string UserId { get; set; }
        public int CategoryId { get; set; }
        public bool IsLiked { get; set; }
    }
}
