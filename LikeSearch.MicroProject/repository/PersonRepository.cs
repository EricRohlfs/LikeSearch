using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LikeSearch;
using Samples.model;
using WebMatrix.Data;

namespace Samples.repository
{
    public class PersonRepository
    {
        public Person GetById()
        {
            return new Person();
        }

        public IEnumerable<Person> GetAll()
        {
            return new List<Person>();
        }

        public IEnumerable<dynamic > Search(out int totalRows,string displayName, int currentPage, int rowsPerPage, bool sortDesc, string orderBy)
        {
            var qb = QueryBuilderFactory.Init("Person");
            //could do one select statement here * or dbo.Person.*
            // but this way shows the fluent mannery in which they can be added.
            qb.AddSelect("PersonId")
                .AddSelect("FirstName")
                .AddSelect("LastName")
                .AddSelect("DisplayName")
                .AddLike("DisplayName", displayName);

            var pager = new PagingFactory(innerQuery: qb, currentPage: currentPage, rowsPerPage: rowsPerPage,
                                          sortDesc: sortDesc, orderBy: orderBy);
            var query = pager.CreateQuery();
            var sqlParams = pager.CreateParameters();
            var countQuery = qb.CreateCount().CreateQuery();
            dynamic data;
            dynamic count;
            using (var db = Database.OpenConnectionString(DataHelper.ConnectionStr(), DataHelper.Provider()))
            {
                data = db.Query(query, sqlParams);
                count  = db.QueryValue(countQuery,sqlParams);
            }
            totalRows = Convert.ToInt32(count);
            return data;
        }
    }
}