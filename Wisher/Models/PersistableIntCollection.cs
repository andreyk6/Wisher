using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisher.Models
{
    [ComplexType]
    public class PersistableIntCollection : PersistableScalarCollection<int>
    {
        protected override int ConvertSingleValueToRuntime(string rawValue)
        {
            return int.Parse(rawValue);
        }

        protected override string ConvertSingleValueToPersistable(int value)
        {
            return value.ToString();
        }
    }

}
