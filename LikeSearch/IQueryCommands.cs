using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LikeSearch
{
    public interface IQueryCommands
    {
        /// <summary>
        /// Webmatrix.data uses params, the will be stacked here untill 
        /// used in webmatrix.data
        /// </summary>
        List<object> SqlParams { get; set; }
        string SelectCommand { get; set; }
        string FromCommand { get; set; }
        string WhereCommand { get; set; }
        string CreateQuery();
    }
}
