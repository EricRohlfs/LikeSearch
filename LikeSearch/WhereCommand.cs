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
    
    public struct WhereItem
    {
        public string WhereExpression { get; set; }
        public object SqlParam { get; set; }
    }
}
