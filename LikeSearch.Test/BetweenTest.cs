using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace LikeSearch.Test
{
    public class BetweenTest
    {
        [Test]
        public void BetweenShouldWork()
        {
            var from = "200";
            var to = "300";
           
            var qb = QueryBuilderFactory.Init("Table1");
            qb.AddSelect("Name")
                .AddWhere("Name","=","John")
                .AddBetween("price", from, to);
            var innerQ = qb.Create().CreateQuery();

            var pagingfactory = new PagingFactory(qb, 1, 10, "Name");
            var result = pagingfactory.CreateQuery();
            var sqlParams = pagingfactory.CreateParameters();

            Console.WriteLine(innerQ);
            Console.WriteLine(result);
            foreach (var sqlParam in sqlParams)
            {
                Console.WriteLine(sqlParam.ToString());
            }

            var expected = "Name =  @0  AND price BETWEEN @1 AND @2 ";
            Assert.IsTrue(innerQ.Contains(expected));

            Assert.AreEqual("John", sqlParams[0], "name");
            var val1 = sqlParams[1].ToString();
            Assert.AreEqual(from, val1 ,"min");
            var val2 = sqlParams[2].ToString();
            Assert.AreEqual(to, val2, "max");

            var endAnd = AllTests.NoEmptyEndAnd(result);
            Assert.IsTrue(endAnd.Result, endAnd.Msg);
           

        }
    }
}
