using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LikeSearch
{
    public class DateSearchCommand
    {

        public static string BetweenExpression = "{0} BETWEEN {1} and {2}";

        /// <summary>
        /// We have three scenarios, 
        /// 1. start and end are null or min, do nothnig
        /// 2. start has value, end dose not, set end to now
        /// 3. start is null or min, and end have a value, everythnig before end.
        /// 
        /// If the name is null then we will not do anything.
        /// </summary>
        /// <param name="dateSearch"></param>
        /// <returns></returns>
        public WhereItem CreateQuery(DateSearch dateSearch)
        {
            var wi = new WhereItem();
            // if there is no property name we can't really do anything.
            if (string.IsNullOrWhiteSpace(dateSearch.PropertyName))
            {
                return wi;
            }

            var fromIsMin = (dateSearch.From.CompareTo(DateTime.MinValue) == 0);
            var toIsMin = (dateSearch.To.CompareTo(DateTime.MinValue) == 0);
            // if the values are min, then they are same as null
            if (fromIsMin && toIsMin) { return wi; }


            if (!dateSearch.VerifySqlFriendly(dateSearch.From))
            {
                dateSearch.From = DateSearch.SqlMin;
            }

            if (!dateSearch.VerifySqlFriendly(dateSearch.To))
            {
                dateSearch.To = DateTime.Now;
            }
            //put the property name in there then send in the other spaces
            var expression = string.Format(BetweenExpression, dateSearch.PropertyName, "{0}", "{1}");
            wi.WhereExpression = expression;
            wi.SqlParams.AddRange(CreateParams(dateSearch));
            return wi;
        }

        //this just makes sure the params are added in the proper order
        private List<object> CreateParams(DateSearch dateSearch)
        {
            var result = new List<object>()
                            {
                               // dateSearch.PropertyName,
                                dateSearch.From,
                                dateSearch.To
                            };
            return result;
        }


    }
}
