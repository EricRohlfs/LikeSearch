using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;
using Samples.repository;

namespace Samples
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BuildGrid();
        }

        private string _orderBy;
        public string OrderBy
        {
            get
            {
                _orderBy = Page.Request.Params.AllKeys.Contains("sort") ? this.Page.Request.Params["sort"] : "FirstName";
                return _orderBy;
            }
            set { _orderBy = value; }
        }

        private bool _sortDesc1;
        public bool SortDesc1
        {
            get
            {
                var dir = Page.Request.Params.AllKeys.Contains("sortdir") ? this.Page.Request.Params["sortdir"] : "ASC";
                _sortDesc1 = (dir.Contains("DESC"));
                return _sortDesc1;
            }
            set { _sortDesc1 = value; }
        }


        private int _currentGridPage;
        public int CurrentGridPage
        {
            get
            {
                var pageNum = Page.Request.Params.AllKeys.Contains("page") ? this.Page.Request.Params["page"] : "1";
                _currentGridPage = Convert.ToInt32(pageNum);
                return _currentGridPage;
            }
            set { _currentGridPage = value; }
        }

        protected void BuildGrid()
        {
            //default.aspx?sort=FirstName&sortdir=DESC&page=1
           
            string displayName = DisplayName.Text;
            var rowsPerPage = 4;
            int totalRows = 0;
            var repo = new PersonRepository();
            var data = repo.Search(out totalRows,displayName: displayName, currentPage: CurrentGridPage, rowsPerPage: rowsPerPage, sortDesc: SortDesc1,
                                   orderBy: OrderBy);
            var columnNames = new List<string>()
                                  {
                                      {"PersonId"},
                                      {"FirstName"},
                                      {"LastName"},
                                      {"DisplayName"}
                                  };

            var wg = new WebGrid(source: data, rowsPerPage: rowsPerPage, canPage: true, canSort: true,
                                 columnNames: columnNames);

            var qs = new NameValueCollection(Request.QueryString);

            var t = totalRows / rowsPerPage;

            var sb = new StringBuilder();
            for (int i = 1; i < t+1; i++)
            {
                if (qs.AllKeys.Contains("page"))
                {
                    qs.Set("page", i.ToString());
                }
                else
                {
                    qs.Add("page", i.ToString());
                }
                var url = ToQueryString(qs);
                var link = string.Format("<a href=\"{0}\">{1}</a>", url,i);
                sb.AppendLine(link);
            }
            
            var test = string.Format("<tfoot><tr><td colspan=\"{0}\">{1} </td></tr></tfoot>", columnNames.Count, sb.ToString());
            
             LiteralGrid1.Text = wg.GetHtml().ToHtmlString();
             LiteralGrid1.Text = LiteralGrid1.Text.Replace("</thead>", test);


        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {

        }

        private string ToQueryString(NameValueCollection nvc) { return "?" + string.Join("&", Array.ConvertAll(nvc.AllKeys, key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(nvc[key])))); } 
    }
}