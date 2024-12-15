using Microsoft.Diagnostics.Instrumentation.Extensions.Intercept;
using QuanLyTaiSan_UserManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QuanLyTaiSan_UserManagement.Common
{
    public class CommonBMT
    {

        public partial class TypeParentTypeChild
        {
            public string NameTypeChildren { get; set; }
            public int TypeSymbolChildren { get; set; }
            public string TypeParent { get; set; }
            public int? IdParent { get; set; }
            public Array numbers { get; set; }
        }

    }
}