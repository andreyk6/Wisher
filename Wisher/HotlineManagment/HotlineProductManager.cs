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
        public static HotlineProductModel GetToProducts(CategoryInfo category)
        {
            try
            {
                HtmlWeb web = new HtmlWeb();

                HtmlDocument document;

                if (category.EbayCategoryId.Contains("hotline"))
                {
                    document = web.Load(category.EbayCategoryId);
                }
                else
                {
                    document = web.Load(@"http://m.hotline.ua" + category.EbayCategoryId);
                }

                var topItemNode = ElementsByClass(document, "a", "list_tovar")[0];
                var topItemData = topItemNode.ChildNodes[1].ChildNodes[1];
                return new HotlineProductModel()
                {
                    HotlineUrl = "http://hotline.ua" + topItemNode.Attributes["href"].Value,
                    ImageUrl = "http://hotline.ua" + topItemData.ChildNodes[1].ChildNodes[0].ChildNodes[0].Attributes["src"].Value,
                    Name = topItemData.ChildNodes[3].ChildNodes[1].InnerText,
                    Price = topItemData.ChildNodes[3].ChildNodes[8].InnerText,
                };
            }
            catch (Exception e)
            {

                return null;
            }
        }

        static HtmlNodeCollection ElementsByClass(HtmlDocument doc, string element, string className)
        {
            return doc.DocumentNode.SelectNodes("//" + element + "[@class='" + className + "']");
        }
    }
}
