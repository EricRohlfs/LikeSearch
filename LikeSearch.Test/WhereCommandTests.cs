using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace LikeSearch.Test
{
   public class WhereCommandTests
    {

       [Test]
       public void WhereCommandDoesNotEndInAnd()
       {
           List<WhereItem> items = new List<WhereItem>();
           items.Add(new WhereItem(){SqlParam = "Test",WhereExpression = "test1={0}"});

           var result = QueryBuilder.WhereCommandBuilder(items);
           Console.WriteLine(result.WhereExpression);

           Assert.IsFalse(Regex.IsMatch(result.WhereExpression,@" and$|AND$"));
       }

       [Test]
       public void WhereCommandDoesNotEndInAnd2()
       {
           var items = new List<WhereItem>();
           items.Add(new WhereItem() { SqlParam = "Test", WhereExpression = "test1={0}" });
           var paramList = new List<object>(){"one","two", "three"};
           items.Add(new WhereItem(){SqlParams = paramList, WhereExpression = "{0} between {1} {2}"});

           var result = QueryBuilder.WhereCommandBuilder(items);
           Console.WriteLine(result.WhereExpression);

           Assert.IsFalse(Regex.IsMatch(result.WhereExpression, @" and$|AND$"));


       }


    }
}
