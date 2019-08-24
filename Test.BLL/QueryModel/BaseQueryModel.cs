using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Service.QueryModel
{
    public class BaseQueryModel
    {
        public BaseQueryModel()
        {
            PageIndex = PageIndex == 0 ? 1 : PageIndex;
            PageSize = PageSize == 0 ? 20 : PageSize;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
