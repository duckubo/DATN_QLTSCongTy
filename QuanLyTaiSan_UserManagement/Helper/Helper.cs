using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace QuanLyTaiSan_UserManagement.Helper
{
    public static class Helper
    {
        public static string GetEnumDescription(this int value, Type e)
        {
            string description = null;
            Array values = Enum.GetValues(e);
            foreach (int val in values)
            {
                if (val != value) continue;
                var memInfo = e.GetMember(e.GetEnumName(val));
                var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                description = descriptionAttributes.Length > 0 ? ((DescriptionAttribute)descriptionAttributes[0]).Description : val.ToString();
                break;
            }
            return description;
        }

        public static bool EqualsIgnoringCase(this string value, string compare)
        {
            return string.Equals(value, compare, StringComparison.OrdinalIgnoreCase);
        }
    }
}