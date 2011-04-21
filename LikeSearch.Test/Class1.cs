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
        public void SunnyDay()
        {
            const string firstName = "Ja";
            const string lastName = "Bond";
            var q = QueryBuilderFactory.Init("Person");
            q.AddSelect("FirstName")
                .AddSelect("LastName")
                .AddLike("FirstName", firstName)
                .AddLike("LastName", lastName);
            
            var pagingFactory = new PagingFactory(innerQuery: q, currentPage:2, rowsPerPage:10 , orderBy:"FirstName");
            var result = pagingFactory.CreateQuery();
            var sqlParams = pagingFactory.CreateParameters();
            Assert.IsTrue(sqlParams.Count() == 2);
            
            var s1 = "With OuterQuery AS (";
            var s2 = "SELECT FirstName,LastName ";
            var s3 = "FROM Person";
            var s4 = "WHERE FirstName LIKE @0 AND LastName LIKE @1";
            var s5 = ") SELECT * ";
            var s6 = "FROM OuterQuery";
            var s7 = "WHERE RowNumber BETWEEN 11 AND 20";
           
            Assert.IsTrue(result.Contains(s1));
            Assert.IsTrue(result.Contains(s2));
            Assert.IsTrue(result.Contains(s3));
            Assert.IsTrue(result.Contains(s4));
            Assert.IsTrue(result.Contains(s5));
            Assert.IsTrue(result.Contains(s6));
            Assert.IsTrue(result.Contains(s7));
            
        }

        [Test]
        public void EmptyStringShouldNotAddLike()
        {

            var firstName = string.Empty;
            var q = QueryBuilderFactory.Init("Person");
            q.AddSelect("FirstName")
                .AddSelect("LastName")
                .AddSelect("DisplayName")
                .AddLike("FirstName", firstName, true);
            var query = q.Create().CreateQuery();
            Console.WriteLine("Query:" + query);
            Assert.IsFalse(query.ToLower().Contains("like"));

        }
    }
    }

