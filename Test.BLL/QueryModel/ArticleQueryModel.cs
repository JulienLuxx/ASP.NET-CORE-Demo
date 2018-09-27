using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Service.QueryModel
{
    public class ArticleQueryModel: BasePageQueryModel
    {
        public int? State { get; set; }

        public int? UserId { get; set; }
    }
}
