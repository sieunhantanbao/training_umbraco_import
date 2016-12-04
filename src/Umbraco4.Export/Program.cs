using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using Umbraco4.Export.Models;
using System.IO;

namespace Umbraco4.Export
{
    public class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\umbraco.config");
            //Japanese News
            Console.WriteLine("Exporting news for Japanese....");
            var japaneseNews = ParseNewsPageXML(path, "1939");
            var pathJapaneseNews = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\JapaneseNews.json");
            var sw = new System.IO.StreamWriter(pathJapaneseNews, true,Encoding.UTF8);
            sw.WriteLine(japaneseNews);
            sw.Close();
            Console.WriteLine("Exported news for Japanese....");
            Console.WriteLine("Exporting news for English....");
            //English News
            var englishNews = ParseNewsPageXML(path, "1928");
            var pathEnglishNews = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\EnglishNews.json");
            var sw1 = new System.IO.StreamWriter(pathEnglishNews, true, Encoding.UTF8);
            sw1.WriteLine(englishNews);
            sw1.Close();
            Console.WriteLine("Exported news for English....");
            Console.WriteLine("Press any key to exit....");
            Console.ReadLine();
        }

        public static string ParseNewsPageXML(string path, string nodeId)
        {
            // Loading from a file, you can also load from a stream
            var xml = XDocument.Load(path);

            var newsPageNode = xml.Root.Descendants("NewsPage").FirstOrDefault(c => c.Attribute("id").Value == nodeId);

            var newsPage = new NewsPageModel();
            newsPage.NodeName = newsPageNode.Attribute("nodeName").Value;
            newsPage.PageName = newsPageNode.Descendants("pageName_").FirstOrDefault().Value;
            newsPage.PageTitle = newsPageNode.Descendants("pageTitle").FirstOrDefault().Value;
            newsPage.MetaKeyword = newsPageNode.Descendants("metaKeywords").FirstOrDefault().Value;
            newsPage.MetaDescription = newsPageNode.Descendants("metaDescription").FirstOrDefault().Value;
            newsPage.HeaderScript = newsPageNode.Descendants("pageHeaderScript").FirstOrDefault().Value;
            newsPage.BodyFooterScript = newsPageNode.Descendants("pageBodyFooterScript").FirstOrDefault().Value;

            newsPage.Categories = new List<CategoryPageModel>();

            foreach (var categoryNode in newsPageNode.Descendants("CategoryPage"))
            {
                var category = new CategoryPageModel();
                category.NodeName = categoryNode.Attribute("nodeName").Value;
                category.PageName = categoryNode.Descendants("pageName_").FirstOrDefault().Value;
                category.PageTitle = categoryNode.Descendants("pageTitle").FirstOrDefault().Value;
                category.MetaKeyword = categoryNode.Descendants("metaKeywords").FirstOrDefault().Value;
                category.MetaDescription = categoryNode.Descendants("metaDescription").FirstOrDefault().Value;
                category.HeaderScript = categoryNode.Descendants("pageHeaderScript").FirstOrDefault().Value;
                category.BodyFooterScript = categoryNode.Descendants("pageBodyFooterScript").FirstOrDefault().Value;

                category.Articles = new List<ArticlePageModel>();
                foreach (var articleNode in categoryNode.Descendants("ArticlePage"))
                {
                    var article = new ArticlePageModel();
                    article.NodeName = articleNode.Attribute("nodeName").Value;
                    article.PageName = articleNode.Descendants("pageName_").FirstOrDefault().Value;
                    article.PageTitle = articleNode.Descendants("pageTitle").FirstOrDefault().Value;
                    article.MetaKeyword = articleNode.Descendants("metaKeywords").FirstOrDefault().Value;
                    article.MetaDescription = articleNode.Descendants("metaDescription").FirstOrDefault().Value;
                    article.HeaderScript = articleNode.Descendants("pageHeaderScript").FirstOrDefault().Value;
                    article.BodyFooterScript = articleNode.Descendants("pageBodyFooterScript").FirstOrDefault().Value;
                    //Article Summary
                    article.IsFeature = articleNode.Descendants("featured").FirstOrDefault().Value=="1"?true:false ;
                    article.Title = articleNode.Descendants("title").FirstOrDefault().Value;
                    article.DateCreated = articleNode.Descendants("dateCreated").FirstOrDefault().Value;
                    article.Author = articleNode.Descendants("authorOrLocation").FirstOrDefault().Value;
                    article.Summary = articleNode.Descendants("summary").FirstOrDefault().Value;
                    article.Thumnail = articleNode.Descendants("thumbnail").FirstOrDefault().Value;
                    //Article
                    var ariticleBodyContainerMacroNode = articleNode.Descendants("bodyContainer").FirstOrDefault().Descendants("container").FirstOrDefault().Descendants("macro").FirstOrDefault(c=>c.Attribute("alias").Value== "article");
                    article.A_Title = ariticleBodyContainerMacroNode.Descendants("parameter").FirstOrDefault(c=>c.Attribute("alias").Value== "title").Value;
                    article.A_Date = ariticleBodyContainerMacroNode.Descendants("parameter").FirstOrDefault(c => c.Attribute("alias").Value == "date").Value;
                    article.A_Author = ariticleBodyContainerMacroNode.Descendants("parameter").FirstOrDefault(c => c.Attribute("alias").Value == "author").Value;
                    article.A_Content = ariticleBodyContainerMacroNode.Descendants("parameter").FirstOrDefault(c => c.Attribute("alias").Value == "content").Value;

                    category.Articles.Add(article);
                }
                newsPage.Categories.Add(category);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(newsPage);
        }
    }
}
