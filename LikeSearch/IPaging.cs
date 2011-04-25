using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LikeSearch
{
    /// <summary>
    /// workflow
    /// grid calls repository -> search-> likesearch -> webmatrix.data
    /// If the repository was called PersonRepository.
    /// I wolud implement a method in that class called Search(...,...,...,...,...)
    /// That method would call a second class PersonSearch.
    /// This is mainly to keep the repository class from becoming too big.
    /// you may want to make a class called PersonSearchView that is a poco class that 
    /// matches the columns you are going to show on the grid. 
  
    /// </summary>
  
   public interface IPaging
    {
       /// <summary>
       /// think data grid you are on page 2 
       /// </summary>
      int CurrentPage { get; set; }

       /// <summary>
       /// the number of items to show on the page
       /// </summary>
      int RowsPerPage { get; set; }

      /// <summary>
      /// think SortOn 
      /// </summary>
      string OrderBy { get; set; }

       /// <summary>
       /// sort descending true of false
       /// </summary>
       bool SortDesc { get; set; }

     //  int TotalRowCount { get; set; }
    }

    /// <summary>
    /// this is probably over engineering, but does add nice structure
    /// </summary>
    public interface IPagingResponse<TItems>
    {
        int TotalRows { get; set; }
        IEnumerable<TItems> Data { get; set; }
        
    }

    /// <summary>
    /// use this interface for dynamic objects, there is one for strongly typed as well
    /// </summary>
    public interface IPagingResponse
    {
        int TotalRows { get; set; }
        dynamic Data { get; set; }

    }
}
