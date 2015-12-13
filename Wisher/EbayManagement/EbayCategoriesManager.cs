using System.Collections.Generic;
using System.Xml;
using Wisher.Models;
using eBay.Service.Call;
using eBay.Service.Core.Soap;

namespace Wisher.EbayManagement
{
    public class EbayCategoriesManager
    {
        public static List<CategoryInfo> GetCategories()
        {
            var apiContext = EbayApiContext.GetApiContext();
            GetCategoriesCall apiCall = new GetCategoriesCall(apiContext);
            apiCall.CategorySiteID = "0";
            apiCall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
            CategoryTypeCollection cats = apiCall.GetCategories();

            return GetCategoriesFromXML(apiCall.CategoryList);
        }

        private static List<CategoryInfo> GetCategoriesFromXML(CategoryTypeCollection cats)
        {
            var result = new List<CategoryInfo>();

            XmlElement topLevelElement = null;
            XmlElement childLevelElement = null;
            XmlNode parentNode = null;
            string categoryString = "";
            XmlDocument returnDoc = new XmlDocument();
            XmlElement root = returnDoc.CreateElement("CategoryArray");
            returnDoc.AppendChild(root);
            XmlNode rootNode = returnDoc.SelectSingleNode("/CategoryArray");
            //Loop through CategoryTypeCollection 

            foreach (CategoryType category in cats)
            {
                result.Add(new CategoryInfo()
                {
                    Level = category.CategoryLevel,
                    Name = category.CategoryName,
                    HotLineParrentCategoryId = int.Parse(category.CategoryParentID[0]),
                    HotLIneCategoryId = int.Parse(category.CategoryID)
                });
            }
            return result;
        }
    }
}
