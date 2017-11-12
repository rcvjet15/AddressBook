using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace AddressBook.Helpers
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Html helper extension method that compares requested route with the given <param name="value" /> value,
        /// if a match is found then <param name="attribute" /> attreibute value is returned else empty string is returned.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="value">Controller or action that will be compared with route.</param>
        /// <param name="attribute">The attribute value that will be returned if match is found.</param>
        /// <returns>A HtmlString containig the given attribute value if match is found else empty string is returned.</returns>
        public static IHtmlString RouteIf(this HtmlHelper helper, string value, string attribute)
        {
            string currentController =
                (helper.ViewContext.RequestContext.RouteData.Values["controller"] ?? String.Empty).ToString();

            string currentAction =
                (helper.ViewContext.RequestContext.RouteData.Values["action"] ?? String.Empty).ToString();

            bool hasController = value.Equals(currentController, StringComparison.InvariantCultureIgnoreCase);
            bool hasAction = value.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase);

            return hasAction || hasController ? new HtmlString(attribute) : new HtmlString(String.Empty);
        }        
    }
}