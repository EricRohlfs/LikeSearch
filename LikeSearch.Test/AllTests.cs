using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace LikeSearch.Test
{
   public class AllTests
    {
     


       public static TestResults NoEmptyEndAnd(string pagedQuery  )
       {
           var ts = new TestResults();
           var noSpaces = pagedQuery.Replace(" ", string.Empty);
           var noNewline = noSpaces.Replace(Environment.NewLine, string.Empty);

           var badString = "AND)";
           ts.Result = !noNewline.Contains(badString);
           ts.Msg = "the where clause (or other) has an AND with a parenthesis right after.";
           Console.WriteLine(noNewline);
           return ts;
       }

    }

    public class TestResults
    {
          public bool Result { get; set; }
       public string Msg { get; set; }
    }
}
