using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LikeSearch
{


    [Serializable]
    public class DateSearch
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public String PropertyName { get; set; }

        public DateSearch()
        {

        }

        /// <summary>
        /// This constructor will convert the datetimes for you.
        /// </summary>
        /// <param name="propertyName">name of the filed from the sql database.</param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public DateSearch(string propertyName,string from, string to)
        {

            var from1 = new DateTime();
            var to1 = new DateTime();
            DateTime.TryParse(from, out from1);
            DateTime.TryParse(to, out to1);
            From = from1;
            To = to1;
            PropertyName = propertyName;

        }

        public bool VerifySqlFriendly(DateTime date)
        {
            if (date.CompareTo(SqlMin) < 0)
            {
                return false;
            }
            return true;

        }

        public static DateTime SqlMin
        {
            get
            {
                return DateTime.Parse(System.Data.SqlTypes.SqlDateTime.MinValue.ToString());
            }
        }

    }
}
