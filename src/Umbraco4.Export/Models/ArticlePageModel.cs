using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco4.Export.Models
{
    public class NewsPageModel
    {
        public string NodeName { get; set; }
        public string PageName { get; set; }
        public string PageTitle { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public string HeaderScript { get; set; }
        public string BodyFooterScript { get; set; }
        public IList<CategoryPageModel> Categories { get; set; }
    }

    public class CategoryPageModel
    {
        public string NodeName { get; set; }
        public string PageName { get; set; }
        public string PageTitle { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public string HeaderScript { get; set; }
        public string BodyFooterScript { get; set; }
        public IList<ArticlePageModel> Articles { get; set; }
    }
    public class ArticlePageModel
    {
        public string NodeName { get; set; }
        public string PageName { get; set; }
        public string PageTitle { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public string HeaderScript { get; set; }
        public string BodyFooterScript { get; set; }

        //Artical Summary
        public bool IsFeature { get; set; }
        public string Title { get; set; }
        public string DateCreated { get; set; }
        public string Author { get; set; }
        public string Summary { get; set; }
        public string Thumnail { get; set; }

        //Primary Center
        public string A_Title { get; set; }
        public string A_Date { get; set; }
        public string A_Author { get; set; }
        public string A_Content { get; set; }
    }
}
