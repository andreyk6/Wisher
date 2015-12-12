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
        
        private static void TransformXMLToNested(CategoryTypeCollection cats)
        {
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
                if (category.CategoryLevel == 1)
                {
                    //Top-level category, so we know we can just add it 
                    topLevelElement = returnDoc.CreateElement("Category");
                    topLevelElement.SetAttribute("Name", category.CategoryName);
                    topLevelElement.SetAttribute("ID", category.CategoryID);
                    rootNode.AppendChild(topLevelElement);
                }
                else
                {
                    // Level number will determine how many Category nodes we are deep 
                    categoryString = "";
                    for (int x = 1; x < category.CategoryLevel; x++)
                    {
                        categoryString += "/Category";
                    }
                    parentNode =
                        returnDoc.SelectSingleNode("/CategoryArray" + categoryString + "[@ID='" +
                                                   category.CategoryParentID[0] + "']");
                    childLevelElement = returnDoc.CreateElement("Category");
                    childLevelElement.SetAttribute("Name", category.CategoryName);
                    childLevelElement.SetAttribute("ID", category.CategoryID);
                    parentNode.AppendChild(childLevelElement);
                }
            }
            returnDoc.Save(@"C:\Temp\EbayCategories-Modified.xml");
        }

    }
}
