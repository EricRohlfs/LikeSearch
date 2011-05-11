using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LikeSearch
{
    public class WhereCommand
    {
        public WhereCommand()
        {
            SqlParameters = new List<object>();
           
        }
        public string WhereExpression { get; set; }
        public List<object> SqlParameters { get; set; }
    } 
    
    public class WhereItem
    {
        public string WhereExpression { get; set; }
        //use this for most calls
        public object SqlParam { get; set; }

        // some object have more than one, such as date search.
        public List<object> SqlParams { get; set; }

        public WhereItem()
        {
            SqlParams = new List<object>(3);
            SqlParam = new object();
        }
    }
}
