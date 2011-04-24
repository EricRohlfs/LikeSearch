using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using WebMatrix.Data;

namespace Samples
{
    public static class DataHelper
    {
        public static string Provider()
        {
            return ConfigurationManager.ConnectionStrings["LikeSearchTest"].ProviderName;
        }
        public static string ConnectionStr()
        {
            return ConfigurationManager.ConnectionStrings["LikeSearchTest"].ToString();
        }
        
    }
}