using BillBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BillBoard.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();

            TagBuilder pLi = new TagBuilder("li");
            TagBuilder pSpan = new TagBuilder("span");

            pSpan.MergeAttribute("aria-hidden", "true");
            pSpan.InnerHtml = "&laquo;";

            if (pagingInfo.CurrentPage == 1)
            {
                pLi.AddCssClass("disabled");
                pLi.InnerHtml = pSpan.ToString();

                result.Append(pLi.ToString());
            }
            else
            {
                TagBuilder pA = new TagBuilder("a");

                pA.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage - 1));
                pA.InnerHtml = pSpan.ToString();
                pLi.InnerHtml = pA.ToString();

                result.Append(pLi.ToString());
            }

            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {

                TagBuilder a = new TagBuilder("a");
                TagBuilder li = new TagBuilder("li");

                a.MergeAttribute("href", pageUrl(i));
                a.InnerHtml = i.ToString();
                li.InnerHtml = a.ToString();

                if (i == pagingInfo.CurrentPage) li.AddCssClass("active");


                result.Append(li.ToString());
            }

            pLi = new TagBuilder("li");

            pSpan.InnerHtml = "&raquo;";

            if (pagingInfo.CurrentPage == pagingInfo.TotalPages)
            {
                pLi.AddCssClass("disabled");
                pLi.InnerHtml = pSpan.ToString();

                result.Append(pLi.ToString());
            }
            else
            {
                TagBuilder pA = new TagBuilder("a");

                pA.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage + 1));
                pA.InnerHtml = pSpan.ToString();
                pLi.InnerHtml = pA.ToString();

                result.Append(pLi.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}