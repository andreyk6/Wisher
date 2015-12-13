using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using HtmlAgilityPack;
using Wisher.Models;

namespace Wisher
{
    public static class HotlineCategoryManager
    {
        public static List<CategoryInfo> GetCategories()
        {
            List<CategoryInfo> categories = new List<CategoryInfo>();

            HtmlWeb web = new HtmlWeb();
            var document = web.Load(@"http://m.hotline.ua/catalog/");
            var firstLevel = ElementsByClass(document, "h3","katalog_list grey_gr");

            foreach (HtmlNode firstCatNode in firstLevel)
            {
                //Get category
                var firstCat = new CategoryInfo()
                {
                    EbayCategoryId = firstCatNode.ChildNodes[0].Attributes["href"].Value,
                    EbayParrentCategoryId = firstCatNode.ChildNodes[0].Attributes["href"].Value,
                    Level = 1,
                    Name = firstCatNode.ChildNodes[0].ChildNodes[0].InnerText
                };
                categories.Add(firstCat);

                //Get nested categories
                var secondLevelDoc = web.Load(@"http://m.hotline.ua" + firstCat.EbayCategoryId);
                var secondLevel = ElementsByClass(secondLevelDoc,"h5", "title_razd");
                var thirdLevel = ElementsByClass(secondLevelDoc,"ul" ,"razd_kat");

                foreach (HtmlNode secondCatListNode in secondLevel)
                {
                    var secondCat = new CategoryInfo()
                    {
                        EbayCategoryId = secondCatListNode.Attributes["id"].Value,
                        EbayParrentCategoryId = firstCat.EbayCategoryId,
                        Level = 2,
                        Name = secondCatListNode.ChildNodes[0].ChildNodes[0].InnerText
                    };
                    categories.Add(secondCat);

                    foreach (var secondCatNode in thirdLevel)
                    {
                        if (secondCatNode.Attributes["id"].Value == secondCatListNode.Attributes["id"].Value.Replace("parent", "sub"))
                        {
                            foreach (var thirdCatNode in secondCatNode.ChildNodes)
                            {
                                if (thirdCatNode.Name != "#text")
                                {
                                    var thirdCat = new CategoryInfo()
                                    {
                                        EbayCategoryId = thirdCatNode.ChildNodes[0].Attributes["href"].Value,
                                        EbayParrentCategoryId = secondCat.EbayCategoryId,
                                        Level = 3,
                                        Name = thirdCatNode.ChildNodes[0].ChildNodes[0].InnerText
                                    };
                                    categories.Add(thirdCat);
                                }
                            }
                        }
                    }
                }

            }

            return categories;
        }
        static HtmlNodeCollection ElementsByClass(HtmlDocument doc, string element,string className)
        {
            return doc.DocumentNode.SelectNodes("//"+element+"[@class='" + className + "']");
        }
    }
}
