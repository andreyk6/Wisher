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
        public static HotlineProductModel GetToProducts(CategoryInfo category, int count = 1)
        {
            HtmlWeb web = new HtmlWeb();

            var document = web.Load(@"http://m.hotline.ua" + category.EbayCategoryId);
            var topItemNode = ElementsByClass(document, "a", "list_tovar")[0];
            var topItemData = topItemNode.ChildNodes[0].ChildNodes[0].ChildNodes[0];
            return new HotlineProductModel()
            {
                HotlineUrl = topItemNode.Attributes["href"].Value,
                ImageUrl = topItemData.ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes["src"].Value,
                Name = topItemData.ChildNodes[0].ChildNodes[1].InnerText,
                Price = topItemData.ChildNodes[0].ChildNodes[4].InnerText,
            };
        }

        static HtmlNodeCollection ElementsByClass(HtmlDocument doc, string element, string className)
        {
            return doc.DocumentNode.SelectNodes("//" + element + "[@class='" + className + "']");
        }
    }
}
