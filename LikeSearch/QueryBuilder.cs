using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LikeSearch
{

    /// <summary>
    /// this class will set everything up in the proper order for you.
    /// </summary>
    public static class QueryBuilderFactory
    {
        /// <summary>
        /// Table or view Name
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableOrViewName"></param>
        /// <returns></returns>
        public static IQueryBuilder Init(string tableOrViewName)
        {
            return new QueryBuilder(new QueryCommands(),tableOrViewName);
        }
    }


    /// <summary>
    /// call the QueryBuilderFactory to create an instance of this class.
    /// </summary>
    public class QueryBuilder : IQueryBuilder
    {

        private IQueryCommands _queryCommandsObj = null;
        public bool SelectDistinct = false;
        protected List<string> SelectFields { get; set; }
        protected List<WhereItem> Where { get; set; }

        public QueryBuilder(IQueryCommands queryCommands, string tableOrViewName)
        {
            
            _queryCommandsObj = queryCommands;
            SelectFields = new List<string>();
            Where = new List<WhereItem>();
            AddFromTableName(tableOrViewName);
        }

        #region Implementation of IQueryBuilder

        public static Func<string, string, string> SelectNameAs = (fieldName, asName) => (asName == null) ? fieldName : string.Format("{0} AS {1}", fieldName, asName);

        public IQueryBuilder AddSelect(string fieldName, string asName = null)
        {
            SelectFields.Add(SelectNameAs(fieldName, asName));
            return this;
        }

        public static Func<string, string> FromExp = (tableName) => string.Format("FROM {0}{1}", tableName, Environment.NewLine);

        [Obsolete("Use constructor")]
        private void AddFromTableName(string tableName)
        {
            _queryCommandsObj.FromCommand = FromExp(tableName);
           // return this;
        }

        /// <summary>
        /// notice the @{0} for the right side of the LIKE command, this is because it will be processed later
        /// as part of an array and the values will be added as parameters, the parameter name will be inserted.
        /// </summary>
        public static Func<string, string> LikeExp = (fieldName) => string.Format("{0} LIKE {1}", fieldName, "@{0}");
        public static Func<string, string> LikeSqlParam = (likeVal) => string.Format("%{0}%", likeVal);

        public IQueryBuilder AddLike(string fieldName, string likeVal, bool excludeIfValIsNullOrWhiteSpace = true)
        {
            if (string.IsNullOrWhiteSpace(likeVal))
            {
                System.Diagnostics.Trace.WriteLine(string.Format("{0} was not added to the like statement because the value was null",fieldName));
                return this;
            }
            var wi = new WhereItem()
            {
                WhereExpression = LikeExp(fieldName),
                SqlParam = LikeSqlParam(likeVal)
            };
            Where.Add(wi);
            return this;
        }


        /// <summary>
        /// where FirstName = "Eric"
        /// "Eric" would be the sql param
        /// FirstName = @{0} should be the expression
        /// @ for parameter and {0} is for the string builder to put the correct param id in there.
        /// </summary>
        /// <param name="sqlParam"> "Eric"</param>
        /// <param name="expression">FirstName = @{0}</param>
        /// <returns></returns>
        public IQueryBuilder AddWhere(string sqlParam, string expression)
        {
            Where.Add(new WhereItem() { SqlParam = sqlParam, WhereExpression = expression });
            return this;
        }

        public IQueryBuilder AddBetween(DateSearch dateSearch)
        {
            var cmd = new DateSearchCommand();
            var wi =  cmd.CreateQuery(dateSearch);
            Where.Add(wi);
            return this;
        }
        public IQueryCommands Create(bool withSelectDistinct = false)
        {
            _queryCommandsObj.SelectCommand = SelectCommandBuilder(SelectFields, withSelectDistinct);
            var wc = WhereCommandBuilder(Where);
            _queryCommandsObj.WhereCommand = wc.WhereExpression;
            _queryCommandsObj.SqlParams = wc.SqlParameters;
            return _queryCommandsObj;
        }

    


        /// <summary>
        /// same as query above except it get a count of the items above for paging
        /// </summary>
        /// <returns></returns>
        public IQueryCommands CreateCount()
        {
            var q = new QueryCommands { SelectCommand = "SELECT Count(*)", FromCommand = _queryCommandsObj.FromCommand };
            var wc = WhereCommandBuilder(Where);
           
            q.WhereCommand = wc.WhereExpression;
            q.SqlParams = wc.SqlParameters;
            return q;
        }
        #endregion

        public static Func<string, bool, string> SelectExp = (csv, distinct) => string.Format("{0} {1} SELECT {2} {3}", Environment.NewLine, (distinct ? " DISTINCT " : ""), csv, Environment.NewLine);

        public static string SelectCommandBuilder(List<string> selectFields, bool selectDistinct)
        {
            var csv = string.Join(",", selectFields.ToArray());
            return SelectExp(csv, selectDistinct);
        }


        public static Func<string, string> WhereExp = (whereItems) => string.Format("WHERE {0} {1} ", whereItems, Environment.NewLine);

        /// <summary>
        /// adds parameters to the sql parameter list 
        /// and builds the where command.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static WhereCommand WhereCommandBuilder(List<WhereItem> expr)
        {

            if (expr.Count < 1) { return new WhereCommand(); }

         

            var wc = new WhereCommand();
            var sb = new StringBuilder();

            foreach (var item in expr)
            {  
                
                var emptyExpr = string.IsNullOrWhiteSpace(item.WhereExpression);
                var paramVal = item.SqlParam.ToString();
                var emptyparam = (string.IsNullOrWhiteSpace(paramVal) || paramVal == "System.Object");
                var emptyparams = (item.SqlParams.Count > 0);
                // if item is essentially null skip it
                if (emptyExpr & emptyparam || emptyparams){continue;}


                //some items have more than one.
                if (item.SqlParams.Count > 0)
                {
                   
                    var collection = new List<object>();
                    foreach (var sqlParam in item.SqlParams)
                    {
                        wc.SqlParameters.Add(sqlParam);
                     } 
                    var i = wc.SqlParameters.Count - item.SqlParams.Count;
                    foreach (var sp in item.SqlParams)
                    {
                        collection.Add("@" + i);
                        i++;
                    }
                    
                    sb.Append(string.Format(item.WhereExpression, collection.ToArray()));
                }
                else
                {
                    if (item.SqlParam != null && item.WhereExpression != null)
                    {
                        wc.SqlParameters.Add(item.SqlParam);
                        sb.Append(string.Format(item.WhereExpression, wc.SqlParameters.Count - 1));
                    }
                }
              
               
                sb.Append(" AND ");
            }


            var wheres = sb.ToString();
            
            var noEndAnd = Regex.Replace(wheres.TrimEnd(), @" and$| AND$", "");
            // just incase others end in and
            while (Regex.IsMatch(noEndAnd,@" and$| AND$"))
            {
                noEndAnd = Regex.Replace(wheres.TrimEnd(), @" and$| AND$", "");
            }

            

            
            // if there are not any items we don't want to add a where
            if (!string.IsNullOrWhiteSpace(noEndAnd))
            {
                wc.WhereExpression = WhereExp(noEndAnd);
            }
            return wc;
        }
    }
}
