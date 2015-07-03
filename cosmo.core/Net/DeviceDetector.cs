using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Cosmo.Net
{
   /// <summary>
   /// Implements a device detector class.
   /// </summary>
   public static class DeviceDetector
   {

      #region Enumerations

      /// <summary>
      /// Enumerate the distinct types of devices.
      /// </summary>
      public enum DeviceType
      {
         /// <summary>Desktop devices (PC, laptop).</summary>
         Desktop,
         /// <summary>Tablet devices.</summary>
         Tablet,
         /// <summary>Smartphone devices.</summary>
         Mobile
      }

      #endregion

      #region Static Members

      public static DeviceType DetectDeviceType(HttpRequest request)
      {
         int tablet_browser = 0;
         int mobile_browser = 0;
         string userAgent =  request.ServerVariables["HTTP_USER_AGENT"].ToLower();
         Regex regex;

         regex = new Regex("/(tablet|ipad|playbook)|(android(?!.*(mobi|opera mini)))/i");
         if (regex.Match(userAgent).Success)
         {
            tablet_browser++;
         }
 
         regex = new Regex("/(up.browser|up.link|mmp|symbian|smartphone|midp|wap|phone|android|iemobile)/i");
         if (regex.Match(userAgent).Success)
         {
            mobile_browser++;
         }
 
         if ((request.ServerVariables["HTTP_ACCEPT"].IndexOf("application/vnd.wap.xhtml+xml") > 0) ||
             (!string.IsNullOrWhiteSpace(request.ServerVariables["HTTP_X_WAP_PROFILE"])) ||
             (!string.IsNullOrWhiteSpace(request.ServerVariables["HTTP_PROFILE"])))
         {
            mobile_browser++;
         }
 
         string mobile_ua = userAgent.Substring(0, 4);
         string[] mobile_agents = { "w3c ", "acs-", "alav", "alca", "amoi", "audi", "avan", "benq", "bird", "blac",
                                    "blaz", "brew", "cell", "cldc", "cmd-", "dang", "doco", "eric", "hipt", "inno",
                                    "ipaq", "java", "jigs", "kddi", "keji", "leno", "lg-c", "lg-d", "lg-g", "lge-",
                                    "maui", "maxo", "midp", "mits", "mmef", "mobi", "mot-", "moto", "mwbp", "nec-",
                                    "newt", "noki", "palm", "pana", "pant", "phil", "play", "port", "prox",
                                    "qwap", "sage", "sams", "sany", "sch-", "sec-", "send", "seri", "sgh-", "shar",
                                    "sie-", "siem", "smal", "smar", "sony", "sph-", "symb", "t-mo", "teli", "tim-",
                                    "tosh", "tsm-", "upg1", "upsi", "vk-v", "voda", "wap-", "wapa", "wapi", "wapp",
                                    "wapr", "webc", "winw", "winw", "xda ", "xda-" };

         if (mobile_agents.Contains(mobile_ua)) 
         {
            mobile_browser++;
         }
 
         if (userAgent.IndexOf("opera mini") > 0) 
         {
            mobile_browser++;

            // Check for tablets on opera mini alternative headers
            string stock_ua = string.Empty;
            if (!string.IsNullOrWhiteSpace(request.ServerVariables["HTTP_X_OPERAMINI_PHONE_UA"])) 
            {
               stock_ua = request.ServerVariables["HTTP_X_OPERAMINI_PHONE_UA"];
            }
            else if (!string.IsNullOrWhiteSpace(request.ServerVariables["HTTP_DEVICE_STOCK_UA"])) 
            {   
               stock_ua = request.ServerVariables["HTTP_DEVICE_STOCK_UA"];
            }
    
            regex = new Regex("/(tablet|ipad|playbook)|(android(?!.*mobile))/i");
            if (regex.Match(stock_ua).Success) 
            {
               tablet_browser++;
            }
         }
 
         if (tablet_browser > 0) 
         {
            Console.WriteLine("Detected access via TABLET device (" + userAgent + ")");
            return DeviceType.Tablet;
         }
         else if (mobile_browser > 0) 
         {
            Console.WriteLine("Detected access via MOBILE/SMARTPHONE device (" + userAgent + ")");
            return DeviceType.Mobile;
         }
         else 
         {
            Console.WriteLine("Detected access via STANDARD device (" + userAgent + ")");
            return DeviceType.Desktop;
         }
      }

      #endregion

   }
}
