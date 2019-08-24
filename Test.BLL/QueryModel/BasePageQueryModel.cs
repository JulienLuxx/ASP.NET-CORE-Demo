using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Service.QueryModel
{
    public class BasePageQueryModel
    {
        public BasePageQueryModel()
        {
            PageIndex = PageIndex == 0 ? 1 : PageIndex;
            PageSize = PageSize == 0 ? 20 : PageSize;
            IsDesc = true;
        }
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }


        private string _orderByColumn = string.Empty;
        public string OrderByColumn
        {
            get => string.IsNullOrEmpty(_orderByColumn) || string.IsNullOrWhiteSpace(_orderByColumn) ? string.Empty : _orderByColumn.Trim();
            set => _orderByColumn = value;
        }

        public bool IsDesc { get; set; }
    }
}
