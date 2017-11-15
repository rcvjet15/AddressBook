using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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

        /// <summary>
        /// Helper that creates Combo box that enables to input data or select from dropdown.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name">Submit value of the field.</param>
        /// <param name="value">Initial display value of the field.</param>
        /// <param name="items">Items that populate dropdown.</param>
        /// <returns>Html string that contains input and dropdown.</returns>
        public static IHtmlString Datalist(this HtmlHelper htmlHelper, string name, string value, IEnumerable<string> items)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"<input type='text' list={name}-list value='{value ?? items.FirstOrDefault()}' class='form-control'/>");
            sb.Append($"<datalist id={name}-list>");
            foreach (string item in items)
            {
                sb.Append($"<option>{item}");
            }
            sb.Append("</datalist>");

            return new HtmlString(sb.ToString());
        }


        /// <summary>
        /// Custom helper that creates Combo box for model intherited from TModel and it's TPorperty that enables to input data or select from dropdown.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="items">Items that populate dropdown.</param>
        /// <returns></returns>
        public static IHtmlString DatalistFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> items)
        {
            return htmlHelper.DatalistFor(expression, items, null);
        }

        /// <summary>
        /// Custom helper that creates Combo box for model intherited from TModel and it's TPorperty that enables to input data or select from dropdown.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="items">Items that populate dropdown.</param>
        /// <param name="htmlAttributes">Html attributes.</param>
        /// <returns></returns>
        public static IHtmlString DatalistFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> items, object htmlAttributes)
        {
            StringBuilder html = new StringBuilder();
            TagBuilder input = new TagBuilder("input");

            ModelMetadata modelMetaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));

            // Input list tag that references datalist
            input.GenerateId(fullName);
            input.Attributes["name"] = fullName;

            // Add html attributes if specified
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                input.MergeAttributes(attributes);
            }

            // Make sure ToString() doesn't throw exception if Model is null
            input.Attributes["value"] = modelMetaData.Model != null ? 
                modelMetaData.Model.ToString() 
                : items.Count() > 0 ? 
                    items.First() : String.Empty;

            // Datalist tag that references input
            StringBuilder datalistHtml = new StringBuilder();

            TagBuilder datalist = new TagBuilder("datalist");
            datalist.GenerateId($"datalist_{fullName}");
            input.Attributes["list"] = datalist.Attributes["id"];

            // Populate the datalist with options
            StringBuilder optionsHtml = new StringBuilder();
            foreach (string item in items)
            {
                TagBuilder option = new TagBuilder("option");
                option.Attributes["value"] = item;
                optionsHtml.Append(option.ToString());
            }
            datalist.InnerHtml = optionsHtml.ToString();

            html.AppendFormat(input.ToString());
            html.AppendLine(datalist.ToString());

            return new HtmlString(html.ToString());
        }
    }
}