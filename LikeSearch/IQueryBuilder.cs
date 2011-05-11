using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LikeSearch
{
    
    public interface IQueryBuilder
    {
        IQueryBuilder AddSelect(string fieldName, string asName = null);
        //IQueryBuilder AddFromTableName(string tableName);
        IQueryBuilder AddLike(string fieldName, string likeVal,bool excludeIfNullOrWhiteSpace = true);

        IQueryBuilder AddBetween(DateSearch dateSearch);
        /// <summary>
        /// For between items set the fieldName as null or sting.empty
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryBuilder AddWhere(string fieldName, string expression);
        /// <summary>
        /// Creates the query
        /// </summary>
        /// <returns></returns>
        IQueryCommands Create(bool withSelectDistinct = false);
        /// <summary>
        /// Creates a count query
        /// </summary>
        /// <returns></returns>
        IQueryCommands CreateCount();
    }

  

   
}
