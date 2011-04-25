using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace LikeSearch
{
    public class PagingFactory:IPaging
    {
        private IQueryBuilder InnerQuery = null;
        #region Implementation of IPaging

        public int CurrentPage { get; set; }
        public int RowsPerPage { get; set; }
        public string OrderBy { get; set; }
        public bool SortDesc { get; set; }
        public int TotalRowCount { get; set; }

        #endregion

        /// <summary>
        /// Must call create first
        /// </summary>
        /// <returns></returns>
        public object[] CreateParameters()
        {
                return InnerQuery.Create().SqlParams.ToArray() ;
        }

        //public PagingFactory()
        //{

        //}

        //public PagingFactory(IQueryBuilder innerQuery)
        //{
        //    InnerQuery = innerQuery;
        //}
        public PagingFactory(IQueryBuilder innerQuery, int currentPage, int rowsPerPage, string orderBy, bool sortDesc = false)
        {
            // Assert(CurrentPage > 0, "CurrentPage must be greater than zero.");
            Contract.Requires<ArgumentOutOfRangeException>(currentPage > 0,
                                                           "The currentPage property must have a value greater than 0");

            Contract.Requires<ArgumentOutOfRangeException>(rowsPerPage > 0,
                                                          "The rowsPerPage property must have a value greater than 0");
            Contract.Requires<ArgumentOutOfRangeException>(string.IsNullOrWhiteSpace(orderBy),"orderBy property must not be null or whitespace, you need to set a default vaule.");

            //ObjectDataSourc/Gridview combo adds dec to the order by for us, 
            //so we want to remove desc from the string and set the sortDesc property to true.
            if (orderBy.Contains(" DESC"))
            {
                sortDesc = true;
                orderBy = orderBy.Replace(" DESC", "");
            }

            InnerQuery = innerQuery;
            CurrentPage = currentPage;
            RowsPerPage = rowsPerPage;
            OrderBy = orderBy;
            SortDesc = sortDesc;
           

            AddRowNumber(OrderBy, SortDesc);
        }
        public PagingFactory(IQueryBuilder innerQuery, IPaging pagingDetails)
        {
            InnerQuery = innerQuery;
            CurrentPage = pagingDetails.CurrentPage;
            RowsPerPage = pagingDetails.RowsPerPage;
            OrderBy = pagingDetails.OrderBy;
            SortDesc = pagingDetails.SortDesc;
            AddRowNumber(OrderBy, SortDesc);
        }


        private Func<int, int, string> BetweenExp = (pageNum, pageSize) => string.Format("RowNumber BETWEEN {0} AND {1}", (pageNum - 1) * pageSize + 1, pageNum * pageSize);

        public virtual string CreateQuery()
        {
            if (CurrentPage == 0)
            {
                CurrentPage = 1;
            }
            //add the row number to the query must be done before we run the inner create query.
            

           var outer = QueryBuilderFactory.Init("OuterQuery");
         
           //we don't need to worry about paramteres in this where expression, we are not passing anything in from the client app.
            outer.AddSelect("*")
                .AddWhere(string.Empty, BetweenExp(CurrentPage,RowsPerPage));
            var innerQstr = InnerQuery.Create().CreateQuery();
            var outerQstr = outer.Create().CreateQuery();
            var outerWithExpr = string.Format("With OuterQuery AS ( {0} ) {1}", innerQstr, outerQstr);
            return outerWithExpr;
        }


        /// <summary>
        /// adds a new select item onto the InnerQuery Factory object.
        /// and creates the new row called RowNumber
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="sortDesc"></param>
        private void AddRowNumber(string orderBy, bool sortDesc = false)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                throw new ArgumentException("You must set order by to a value in your table");
            }
            var f = string.Format("Row_Number() over (order by {0} {1}) ", orderBy, sortDesc ? "DESC" : "ASC");
            InnerQuery.AddSelect(f, "RowNumber");
        }

       
    }
}
