using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WebMatrix.Data;

namespace LikeSearch.Test
{
    public class FirstTests
    {

        [Test]
        public void WhereClauseEmpty()
        {
            var firstName = string.Empty;
            var lastName = string.Empty;
            var ds = new DateSearch("Created", "", "");
            var q = QueryBuilderFactory.Init("Person");
            q.AddSelect("FirstName")
                .AddSelect("LastName")
                .AddLike("FirstName", firstName)
                .AddLike("LastName", lastName)
                .AddBetween(ds);
                

            var pagingFactory = new PagingFactory(innerQuery: q, currentPage: 2, rowsPerPage: 10, orderBy: "FirstName");
            var result = pagingFactory.CreateQuery();
            Console.WriteLine(result);
            var sqlParams = pagingFactory.CreateParameters();
            Assert.IsTrue(sqlParams.Count() == 0);
            var expectedNoEmptyWhere = "WHERE   ";
            Assert.IsFalse(result.Contains(expectedNoEmptyWhere));

        }


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
            var s2 = "SELECT FirstName,LastName";
            var s3 = "FROM Person";
            var s4 = "WHERE FirstName LIKE @0 AND LastName LIKE @1";
            var s5 = ") SELECT * ";
            var s6 = "FROM OuterQuery";
            var s7 = "WHERE RowNumber BETWEEN 11 AND 20";
            var s8 = "RowNumber";
           
            Assert.IsTrue(result.Contains(s1));
            Assert.IsTrue(result.Contains(s2));
            Assert.IsTrue(result.Contains(s3));
            Assert.IsTrue(result.Contains(s4));
            Assert.IsTrue(result.Contains(s5));
            Assert.IsTrue(result.Contains(s6));
            Assert.IsTrue(result.Contains(s7));
            Assert.IsTrue(result.Contains(s8));
            
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

        [Test]
        public void SimpleIntegrationTest()
        {
            var connStr =  ConfigurationManager.ConnectionStrings["MyConnStr"].ToString();
            var proVider = ConfigurationManager.ConnectionStrings["MyConnStr"].ProviderName;

            const string firstName = "Ja";
            const string lastName = "Bond";
            var qb = QueryBuilderFactory.Init("Person");
                qb.AddSelect("FirstName")
                 .AddSelect("LastName")
                 .AddLike("FirstName", firstName)
                 .AddLike("LastName", lastName);
            
            var pagingFactory = new PagingFactory(innerQuery: qb, currentPage:2, rowsPerPage:10 , orderBy:"FirstName");
            var query = pagingFactory.CreateQuery();
            var sqlParams = pagingFactory.CreateParameters();
            dynamic data;
            using (var db = Database.OpenConnectionString(connStr, proVider))
            {
                data = db.Query(query, sqlParams);
            }

            foreach (var item in data)
            {
                Console.WriteLine(String.Format("First: {0} Last{1}", data.FirstName,data.LastName));
            }
            
        }
    }
    }

