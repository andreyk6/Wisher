using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Wisher.Models;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
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
                    EbayParrentCategoryId = int.Parse(category.CategoryParentID[0]),
                    EbayCategoryId = int.Parse(category.CategoryID)
                });
            }
            return result;
        }
    }
}
