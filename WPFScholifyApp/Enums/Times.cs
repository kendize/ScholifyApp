using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScholifyApp.Enums
{
    public class Times
    {
        private static readonly DateTime BaseDate = DateTime.MinValue.Date.ToUniversalTime();

        public static readonly DateTime t8_30 = BaseDate.Add(new TimeSpan(8, 30, 0)).ToUniversalTime();
        public static readonly DateTime t9_15 = BaseDate.Add(new TimeSpan(9, 15, 0)).ToUniversalTime();

        public static readonly DateTime t9_30 = BaseDate.Add(new TimeSpan(9, 30, 0)).ToUniversalTime();
        public static readonly DateTime t10_15 = BaseDate.Add(new TimeSpan(10, 15, 0)).ToUniversalTime();

        public static readonly DateTime t10_30 = BaseDate.Add(new TimeSpan(10, 30, 0)).ToUniversalTime();
        public static readonly DateTime t11_15 = BaseDate.Add(new TimeSpan(11, 15, 0)).ToUniversalTime();

        public static readonly DateTime t11_35 = BaseDate.Add(new TimeSpan(11, 35, 0)).ToUniversalTime();
        public static readonly DateTime t12_20 = BaseDate.Add(new TimeSpan(12, 20, 0)).ToUniversalTime();

        public static readonly DateTime t12_40 = BaseDate.Add(new TimeSpan(12, 40, 0)).ToUniversalTime();
        public static readonly DateTime t13_25 = BaseDate.Add(new TimeSpan(13, 25, 0)).ToUniversalTime();

        public static readonly DateTime t13_35 = BaseDate.Add(new TimeSpan(13, 35, 0)).ToUniversalTime();
        public static readonly DateTime t14_20 = BaseDate.Add(new TimeSpan(14, 20, 0)).ToUniversalTime();

        public static readonly DateTime t14_35 = BaseDate.Add(new TimeSpan(14, 35, 0)).ToUniversalTime();
        public static readonly DateTime t15_20 = BaseDate.Add(new TimeSpan(15, 20, 0)).ToUniversalTime();

        public static readonly DateTime t15_30 = BaseDate.Add(new TimeSpan(15, 30, 0)).ToUniversalTime();
        public static readonly DateTime t16_15 = BaseDate.Add(new TimeSpan(16, 15, 0)).ToUniversalTime();
    }
}
