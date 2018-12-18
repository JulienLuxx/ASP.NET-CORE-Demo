using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Service.QueryModel
{
    public class ArticleQueryModel: BasePageQueryModel
    {
        public int? Status { get; set; }

        public int? UserId { get; set; }

        public string TypeName { get; set; }
    }
}
