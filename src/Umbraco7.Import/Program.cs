using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco7.Import.Models;

namespace Umbraco7.Import
{
    public class Program
    {
        static HttpClient client = new HttpClient();
        static string ApiUmbracoBaseUrl = ConfigurationManager.AppSettings["ApiUmbracoBaseUrl"];

        static void Main(string[] args)
        {
            var pathJapaneseNews = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\JapaneseNews.json");
            int homeNodeId = 1063;
            RunAsync(pathJapaneseNews, homeNodeId).Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }



        public static async Task<int> InsertNewsPageAsync(NewsPageModel model)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/Umbraco/api/news/ImportNewsPage", model);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            int nodeId = await response.Content.ReadAsAsync<int>();
            return nodeId;
        }

        public static async Task<int> InsertCategoryPageAsync(CategoryPageModel model)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/Umbraco/api/news/ImportCategoryPage", model);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            int nodeId = await response.Content.ReadAsAsync<int>();
            return nodeId;
        }

        public static async Task<bool> InsertArticlePageAsync(ArticlePageModel model)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/Umbraco/api/news/ImportArticlePage", model);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return true;
        }

        static async Task RunAsync(string filePath, int languageNodeId)
        {
            client.BaseAddress = new Uri(ApiUmbracoBaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                //Read and parse Json file
               
                var news = GetNews(filePath);

                NewsPageModel newsModel = new NewsPageModel();
                newsModel.ParentId = languageNodeId;//Home node: 1063
                newsModel.NodeName = news.NodeName;
                newsModel.PageName = news.PageName;
                newsModel.PageTitle = news.PageTitle;
                newsModel.MetaKeyword = news.MetaKeyword;
                newsModel.MetaDescription = news.MetaDescription;
                newsModel.HeaderScript = news.HeaderScript;
                newsModel.BodyFooterScript = news.BodyFooterScript;
                int newsNodeInsertedId = await InsertNewsPageAsync(newsModel);
                Console.WriteLine("Insert News: {0} =>Successfully", news.NodeName);
                //Insert Categories
                foreach (var category in news.Categories)
                {
                    CategoryPageModel categoryModel = new CategoryPageModel();
                    categoryModel.ParentId = newsNodeInsertedId;
                    categoryModel.NodeName = category.NodeName;
                    categoryModel.PageName = category.PageName;
                    categoryModel.PageTitle = category.PageTitle;
                    categoryModel.MetaKeyword = category.MetaKeyword;
                    categoryModel.MetaDescription = category.MetaDescription;
                    categoryModel.HeaderScript = category.HeaderScript;
                    categoryModel.BodyFooterScript = category.BodyFooterScript;

                    var categoryNodeInsertedId = await InsertCategoryPageAsync(categoryModel);
                    Console.WriteLine("Insert category: {0} =>Successfully", category.NodeName);
                    //Insert Articles
                    foreach (var article in category.Articles)
                    {
                        ArticlePageModel articleModel = new ArticlePageModel();
                        articleModel.ParentId = categoryNodeInsertedId;
                        articleModel.NodeName = article.NodeName;
                        articleModel.PageName = article.PageName;
                        articleModel.PageTitle = article.PageTitle;
                        articleModel.MetaKeyword = article.MetaKeyword;
                        articleModel.MetaDescription = article.MetaDescription;
                        articleModel.HeaderScript = article.HeaderScript;
                        articleModel.BodyFooterScript = article.BodyFooterScript;

                        articleModel.IsFeature = article.IsFeature;
                        articleModel.Title = article.Title;
                        articleModel.DateCreated = article.DateCreated;
                        articleModel.Author = article.Author;
                        articleModel.Summary = article.Summary;
                        articleModel.Thumnail = article.Thumnail;

                        articleModel.A_Title = article.A_Title;
                        articleModel.A_Date = article.A_Date;
                        articleModel.A_Author = article.A_Author;
                        articleModel.A_Content = article.A_Content;

                        var isInsertArticleSuccess = await InsertArticlePageAsync(articleModel);
                        if (isInsertArticleSuccess)
                        {
                            Console.WriteLine(string.Format("Insert: {0}.=> Success", article.NodeName));
                        }else
                        {
                            Console.WriteLine(string.Format("Insert: {0}.=> Failed", article.NodeName));
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static NewsPageJsonModel GetNews(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;
            using (StreamReader stream = File.OpenText(path))
            {
                using (JsonTextReader reader = new JsonTextReader(stream))
                {
                    reader.SupportMultipleContent = true;

                    var serializer = new JsonSerializer();
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            NewsPageJsonModel news = serializer.Deserialize<NewsPageJsonModel>(reader);
                            if (news != null)
                            {
                                return news;
                            }
                        }
                    }
                }
            }
            return null;
        }

    }
}
