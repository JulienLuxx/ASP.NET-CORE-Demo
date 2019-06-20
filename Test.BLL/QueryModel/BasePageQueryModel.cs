using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Service.QueryModel
{
    public class BasePageQueryModel
    {
        public BasePageQueryModel()
        {
            Page = Page == 0 ? 1 : Page;
            PageSize = PageSize == 0 ? 20 : PageSize;
        }
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public string OrderByColumn { get; set; }

        public bool IsDesc { get; set; }
    }
}
