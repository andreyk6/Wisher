using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Wisher.Models;

namespace Wisher.HotlineManagment
{
    public class HotlineProductManager
    {
        public static HotlineProduct GetToProducts(CategoryInfo category, int count = 1)
        {
            List<CategoryInfo> categories = new List<CategoryInfo>();

            HtmlWeb web = new HtmlWeb();
            var document = web.Load(@"http://m.hotline.ua" + category.EbayCategoryId);
            var firstLevel = ElementsByClass(document, "a", "list_tovar");

            return null;
        }

        static HtmlNodeCollection ElementsByClass(HtmlDocument doc, string element, string className)
        {
            return doc.DocumentNode.SelectNodes("//" + element + "[@class='" + className + "']");
        }
    }

    public class HotlineProduct
    {
        
    }
}
