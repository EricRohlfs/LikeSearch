using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
namespace LikeSearch.Test
{
   public class DateTesting
    {
       [Test]
       public void DateBetweenSunnyDayInnerQueryTest()
       {

           var start = new DateTime(2011, 11, 5);
           var end = new DateTime(2011, 12, 5);
           var ds = new DateSearch("Created", start.ToShortDateString(), end.ToShortDateString());
           const string name = "Dave";
           var qb = QueryBuilderFactory.Init("Table1");
           qb.AddSelect("Name")
               .AddWhere(name, "Name = @{0}")
               .AddBetween(ds);
           var innerQ = qb.Create().CreateQuery();

           var pagingfactory = new PagingFactory(qb, 1, 10, name);
           var result = pagingfactory.CreateQuery();
           var sqlParams = pagingfactory.CreateParameters();

           Console.WriteLine(innerQ);
           Console.WriteLine(result);
           foreach (var sqlParam in sqlParams)
           {
               Console.WriteLine(sqlParam.ToString());
           }

           var expected = "Name = @0 AND Created BETWEEN @1 and @2";
           Assert.IsTrue(innerQ.Contains(expected));

           Assert.AreEqual(name, sqlParams[0], "name");
           var date1 = sqlParams[1].ToString();
           Assert.IsTrue(date1.Contains("11/5/2011"), "date1");
           var date2 = sqlParams[2].ToString();
           Assert.IsTrue(date2.Contains("12/5/2011"), "date2");

           var endAnd= AllTests.NoEmptyEndAnd(result);
           Assert.IsTrue(endAnd.Result,endAnd.Msg);


       }

     /// <summary>
     /// this should not add the between expression because sql min to now is same as no expression
     /// </summary>
       [Test]
       public void DateBetweenNoStartDateOrEndDate()
       {
         // empty string for started and end date
           var ds = new DateSearch("Created", "","");
           var name = "Dave";
           var qb = QueryBuilderFactory.Init("Table1");
           qb.AddSelect("Name")
               .AddWhere(name, "Name = @{0}")
               .AddBetween(ds);
           var innerQ = qb.Create().CreateQuery();

           var pagingfactory = new PagingFactory(qb, 1, 10, name);
           var result = pagingfactory.CreateQuery();
           var sqlParams = pagingfactory.CreateParameters();

           Console.WriteLine(innerQ);
           Console.WriteLine(result);
           foreach (var sqlParam in sqlParams)
           {
               Console.WriteLine(sqlParam.ToString());
           }

           var expected = "Name = @0 AND Created BETWEEN @1 and @2";
           Assert.IsTrue(!innerQ.Contains(expected));
           Assert.AreEqual(sqlParams.Count() ,1);
           //Assert.AreEqual(name, sqlParams[0], "name");
           //var date1 = sqlParams[1].ToString();
           //var dat1Expected = DateSearch.SqlMin.ToString();
           //Assert.AreEqual(dat1Expected,date1, "date1 failed");
           //var date2 = sqlParams[2].ToString();
           //var date2Expected = DateTime.Today;
           //Assert.AreEqual(date2Expected, date2, "date2 failed");


       }

       [Test]
       public void DateNoStartDate()
       {
           // empty string for started and end date
           var ds = new DateSearch("Created", "", "12/5/2011");
           var name = "Dave";
           var qb = QueryBuilderFactory.Init("Table1");
           qb.AddSelect("Name")
               .AddWhere(name, "Name = @{0}")
               .AddBetween(ds);
           var innerQ = qb.Create().CreateQuery();

           var pagingfactory = new PagingFactory(qb, 1, 10, name);
           var result = pagingfactory.CreateQuery();
           var sqlParams = pagingfactory.CreateParameters();

           Console.WriteLine(innerQ);
           Console.WriteLine(result);
           foreach (var sqlParam in sqlParams)
           {
               Console.WriteLine(sqlParam.ToString());
           }

           var expected = "Name = @0 AND Created BETWEEN @1 and @2";
           Assert.IsTrue(innerQ.Contains(expected));
           Assert.AreEqual(sqlParams.Count(), 3);
           Assert.AreEqual(name, sqlParams[0], "name");
           var date1 = sqlParams[1].ToString();
           var dat1Expected = "1/1/1753";
           Assert.IsTrue(date1.Contains(dat1Expected), "date1 failed");
         
       }


       [Test]
       public void DateNoEndReturnsToday()
       {
           // empty string for started and end date
           var ds = new DateSearch("Created", "12/5/2011","");
           var name = "Dave";
           var qb = QueryBuilderFactory.Init("Table1");
           qb.AddSelect("Name")
               .AddWhere(name, "Name = @{0}")
               .AddBetween(ds);
           var innerQ = qb.Create().CreateQuery();

           var pagingfactory = new PagingFactory(qb, 1, 10, name);
           var result = pagingfactory.CreateQuery();
           var sqlParams = pagingfactory.CreateParameters();

           Console.WriteLine(innerQ);
           Console.WriteLine(result);
           foreach (var sqlParam in sqlParams)
           {
               Console.WriteLine(sqlParam.ToString());
           }

           var expected = "Name = @0 AND Created BETWEEN @1 and @2";
           Assert.IsTrue(innerQ.Contains(expected));
           Assert.AreEqual(sqlParams.Count(), 3);
           Assert.AreEqual(name, sqlParams[0], "name");
           var paramVal = sqlParams[2].ToString();
           var date2 = new DateTime();
           DateTime.TryParse(paramVal,out date2);

           Assert.AreEqual(date2.ToShortDateString(),DateTime.Now.ToShortDateString(), "date2 failed");

       }

      
    }
}
