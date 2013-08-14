using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace TaskTimer.UsefulClasses
{
    public static class MyHtmlHelperExtensions
    {
        //image links
        public static MvcHtmlString ImageLink(this HtmlHelper htmlHelper, string imgSrc, string alt, string actionName, string controllerName, object routeValues, object htmlAttributes, object imgHtmlAttributes)
        {
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            var imgtag = MyHtmlHelperExtensions.Image(htmlHelper, imgSrc, alt, imgHtmlAttributes).ToHtmlString();
            string url = urlHelper.Action(actionName, controllerName, routeValues);
            TagBuilder imglink = new TagBuilder("a");
            imglink.MergeAttribute("href", url);
            imglink.InnerHtml = imgtag;
            imglink.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
            return new MvcHtmlString(imglink.ToString());
        }

        public static MvcHtmlString Image(this HtmlHelper htmlHelper, string imgSrc, string alt, object htmlAttributes)
        {
            TagBuilder img = new TagBuilder("img");
            img.MergeAttribute("src", imgSrc);
            img.MergeAttribute("alt", alt);
            img.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
            return new MvcHtmlString(img.ToString());
        }

        public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string url, object htmlAttributes) 
        {
            TagBuilder a = new TagBuilder("a");
            a.MergeAttribute("href", url);
            a.InnerHtml = linkText;
            a.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
            return new MvcHtmlString(a.ToString());

        }

        //display for model
        //public static MvcHtmlString DisplayNameFor<TModel, TProperty>(this HtmlHelper<IEnumerable<TModel>> helper, Expression<Func<TModel, TProperty>> expression)
        //{
        //    var name = ExpressionHelper.GetExpressionText(expression);
        //    name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
        //    var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(() => Activator.CreateInstance<TModel>(), typeof(TModel), name);
        //    return new MvcHtmlString(metadata.DisplayName);
        //}

        //menu ajax action link
        //public static MvcHtmlString DisplayAjaxActioLink(this AjaxHelper htmlHelper, string linkText, string action, string controller, AjaxOptions ajaxOptions)
        //{
        //    UrlHelper qwer1 = ((Controller)htmlHelper.ViewContext.Controller).Url;
        //  /*  qwer1.Action
                




        //    UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
        //    var imgtag = MyHtmlHelperExtensions.Image(htmlHelper, imgSrc, alt, imgHtmlAttributes).ToHtmlString();
        //    string url = urlHelper.Action(actionName, controllerName, routeValues);
        //    TagBuilder imglink = new TagBuilder("a");
        //    imglink.MergeAttribute("href", url);
        //    imglink.InnerHtml = imgtag;
        //    imglink.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
        //    return new MvcHtmlString(imglink.ToString());*/
        //    return null;
        //}
    }
}