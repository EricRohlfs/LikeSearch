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

        IQueryBuilder AddBetween(string fieldName, string value1, string value2);


        /// <summary>
        /// For between items set the fieldName as null or sting.empty
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        [Obsolete("use the other one it's clearer.")]
        IQueryBuilder AddWhere(string fieldName, string expression);


        /// <summary>
        /// adds simple where statement to query
        /// </summary>
        /// <param name="sqlField">e.g. FirstName</param>
        /// <param name="operationSymbol">greater than, less than, equal too</param>
        /// <param name="paramValue">"Chuck"</param>
        /// <param name="skipOnEmptyParamValue"></param>
        /// <returns></returns>
        IQueryBuilder AddWhere(string sqlField, string operationSymbol, string paramValue, bool skipOnEmptyParamValue = true);

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
