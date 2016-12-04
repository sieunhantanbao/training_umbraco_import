using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;
using Umbraco7.Import.Models;

namespace Umbraco7.Controllers
{
    public class NewsController : UmbracoApiController
    {
        [HttpGet]
        public IHttpActionResult GetExample()
        {
            return Ok("You got it");
        }

        [HttpPost]
        public IHttpActionResult ImportNewsPage(NewsPageModel model)
        {
            // Get the Umbraco Content Service
            var contentService = Services.ContentService;
            var newsPage = contentService.CreateContent(
                model.NodeName, // the name of the product
                model.ParentId, // the parent id should be the id of the group node 
                "NewsPage", // the alias of the Document Type
                0);
            // We need to update properties
            newsPage.SetValue("pageName", model.PageName);
            newsPage.SetValue("pageTitle", model.PageTitle);
            newsPage.SetValue("metaKeyword", model.MetaKeyword);
            newsPage.SetValue("metaDescription", model.MetaDescription);
            newsPage.SetValue("headerScript", HttpUtility.HtmlDecode(model.HeaderScript));
            newsPage.SetValue("bodyFooterScript", HttpUtility.HtmlDecode(model.BodyFooterScript));

            // finally we need to save and publish it (which also saves the product!) - that's done via the Content Service
            var result = contentService.SaveAndPublishWithStatus(newsPage);
            if (result.Success)
                return Ok(result.Result.ContentItem.Id);
            else
                return Ok(-1);

        }

        [HttpPost]
        public IHttpActionResult ImportCategoryPage(CategoryPageModel model)
        {
            // Get the Umbraco Content Service
            var contentService = Services.ContentService;
            var newsPage = contentService.CreateContent(
                model.NodeName, // the name of the product
                model.ParentId, // the parent id should be the id of the group node 
                "CategoryPage", // the alias of the Document Type
                0);
            // We need to update properties
            newsPage.SetValue("pageName", model.PageName);
            newsPage.SetValue("pageTitle", model.PageTitle);
            newsPage.SetValue("metaKeyword", model.MetaKeyword);
            newsPage.SetValue("metaDescription", model.MetaDescription);
            newsPage.SetValue("headerScript", HttpUtility.HtmlDecode(model.HeaderScript));
            newsPage.SetValue("bodyFooterScript", HttpUtility.HtmlDecode(model.BodyFooterScript));

            // finally we need to save and publish it (which also saves the product!) - that's done via the Content Service
            var result = contentService.SaveAndPublishWithStatus(newsPage);
            if (result.Success)
                return Ok(result.Result.ContentItem.Id);
            else
                return Ok(-1);

        }

        [HttpPost]
        public IHttpActionResult ImportArticlePage(ArticlePageModel model)
        {
            // Get the Umbraco Content Service
            var contentService = Services.ContentService;
            var articlePage = contentService.CreateContent(
                model.NodeName, // the name of the product
                model.ParentId, // the parent id should be the id of the group node 
                "ArticlePage", // the alias of the Document Type
                0);
            // We need to update properties
            articlePage.SetValue("pageName", model.PageName);
            articlePage.SetValue("pageTitle", model.PageTitle);
            articlePage.SetValue("metaKeyword", model.MetaKeyword);
            articlePage.SetValue("metaDescription", model.MetaDescription);
            articlePage.SetValue("headerScript", HttpUtility.HtmlDecode(model.HeaderScript));
            articlePage.SetValue("bodyFooterScript", HttpUtility.HtmlDecode(model.BodyFooterScript));

            articlePage.SetValue("isFeature", model.IsFeature);
            articlePage.SetValue("title", model.Title);
            articlePage.SetValue("dateCreated", model.DateCreated);
            articlePage.SetValue("author", model.Author);
            articlePage.SetValue("summary", model.Summary);
            //articlePage.SetValue("thumnail", thumbnail);
            articlePage.SetValue("articleTitle", model.A_Title);
            articlePage.SetValue("articleDate", model.A_Date);
            articlePage.SetValue("articleAuthor", model.A_Author);
            articlePage.SetValue("articleContent", model.A_Content);

            // finally we need to save and publish it (which also saves the product!) - that's done via the Content Service
            var result = contentService.SaveAndPublishWithStatus(articlePage);
            if (result.Success)
                return Ok(true);
            else
                return Ok(false);

        }
    }
}