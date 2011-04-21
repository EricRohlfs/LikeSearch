using System;
using System.Collections.Generic;
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
            InnerQuery = innerQuery;
            CurrentPage = currentPage;
            RowsPerPage = rowsPerPage;
            OrderBy = orderBy;
            SortDesc = sortDesc;
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
            var f = string.Format("Row_Number() over (order by {0} {1}) ", orderBy, sortDesc ? "DESC" : "ASC");
            InnerQuery.AddSelect(f, "RowNumber");
        }

       
    }
}
