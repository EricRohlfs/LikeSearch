using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LikeSearch
{
   public class PagingRequestDetails:IPaging
    {
       #region Implementation of IPaging

       public int CurrentPage { get; set; }
       public int RowsPerPage { get; set; }
       public string OrderBy { get; set; }
       public bool SortDesc { get; set; }
       public int TotalRowCount { get; set; }

       #endregion

       public PagingRequestDetails()
       {
           
       }
       public PagingRequestDetails(int currentPage, int rowsPerPage, string orderBy, bool sortDesc)
       {
           CurrentPage = currentPage;
           RowsPerPage = rowsPerPage;
           OrderBy = orderBy;
           SortDesc = sortDesc;
       }
    }
}
