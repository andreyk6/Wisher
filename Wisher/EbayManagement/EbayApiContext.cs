using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;

namespace Wisher.EbayManagement
{
    public class EbayApiContext
    {
        private static string _serverUrl = @"https://api.ebay.com/wsapi";
        private static string _ebayTocken = "AgAAAA**AQAAAA**aAAAAA**ojhsVg**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AFmYOoAZOCoQudj6x9nY+seQ**FxsDAA**AAMAAA**EAegW+5JpzmSok0/+TPFigNa/ySga1IC0aD/z7TMjs71EuiScZMT9gUQuNxkPRlW7JnqW3r0j+V/HJFSdNk6o/9XN9z23iR0KSJ7pDpqOpJJnLXNqGKObBdfHKrlYSfpKAD/DyStKEsz6za3PWh8ABgio0i8bt5wQOwBF004aKC2WGqMj0hJ4ePu083cNTczG3a+it3baz3Brc7cJvocwpzMrcY8Z5SVLVHcGWwL/Da8vFBA4Fyn2GxhS1+2UGx84iGj9pkRD+KKd3atWTME2St3MnrErQqFoqvmlSJTSoekxUnMZgCJKkorhasPX4LCYwdhzkYIVP0jC+H4OBG0uXNOi4FqD78cyE5ZhmMh4OU8q3zAN/sXL+hC8oHX3hDw35jWEXWwAvWwG2VD3VakCkQNXtYtuNdVevA+dPfLDeSqVcnHhnal+EBBcK6x51flAoiWx8e0zAxAnc8/3qpl48CFQV0O2cl4I8HrmdVAcDoSHtavT/ytyrBHmA8sII9V53cOXYuFWAsleoALaDt7WcF8TG/dqki3ryooyIgFwwrNxtqbMIiUt1g3PGNVTUYlP8a3Umqb7GjaQfs632CIifq6hmOSeTJRetl0kVP246V6T8l1xNnlHDyx0Ry1fm+YLi5Zd1gLEISVwNi6/Zus21EqR5ihvN+IMOFWRchORxaWVA4n2ZkGmTS0WJnWPHfoxtwHBiRRPOtRJ8eSiXAh+bbeEyb3pDx5nvk8t6sqjcB2Sv4VeVR0qLzyxZufvhSm";

        private static ApiContext _apiContext;

        public static ApiContext GetApiContext()
        {
            if (_apiContext != null)
            {
                return _apiContext;
            }
            else
            {
                _apiContext = new ApiContext();
            
                //set Api Server Url 
                _apiContext.SoapApiServerUrl = _serverUrl;
                
                //set Api Token to access eBay Api Server 
                ApiCredential apiCredential = new ApiCredential();
                apiCredential.eBayToken = _ebayTocken;
                _apiContext.ApiCredential = apiCredential;
                
                //set eBay Site target to US 
                _apiContext.Site = SiteCodeType.US;
                return _apiContext;
            }
        }
    }
}
