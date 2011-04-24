using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LikeSearch
{
    public class QueryCommands : IQueryCommands
    {
        public List<object> SqlParams { get; set; }

        public string SelectCommand { get; set; }
        public string FromCommand { get; set; }
        public string WhereCommand { get; set; }
        public string CreateQuery()
        {
          
            var sf = string.Format("{0} {1} {2} ", SelectCommand.TrimStart(), FromCommand.TrimStart(), WhereCommand);
            return sf;
        }
    }
}
