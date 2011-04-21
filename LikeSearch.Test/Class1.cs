using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace LikeSearch.Test
{
    public class Class1
    {

          [Test]
        public void test1()
        {
            const string propNameSerach = "The";
            const string citySearch = "Norfolk";

            var q = QueryBuilderFactory.Init("ReListingMaster");
              q.AddSelect("ListingMasterId")
                  .AddSelect("ListingType");
                
            
            if (!string.IsNullOrWhiteSpace(propNameSerach))
            {
                q.Like("PropertyName", propNameSerach);
            }

            if (!string.IsNullOrWhiteSpace(citySearch))
            {
                q.Like("City", citySearch);
            }
          
            var query =  q.Create();
            var qwithPaging = new PagingFactory(q, 2, 10, "ListingType");
              var result = qwithPaging.CreateQuery();
            foreach (var sqlParam in qwithPaging.CreateParameters())
            {
                Console.WriteLine("Param:" +sqlParam );
            }
            Console.WriteLine(query.CreateQuery());
            Console.WriteLine(q.CreateCount().CreateQuery());

            Console.WriteLine(result);
            
        }
    }
    }

