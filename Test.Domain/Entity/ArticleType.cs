﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Domain.Entity
{
    public class ArticleType : IEntity
    {
        public ArticleType()
        {
            Articles = new HashSet<Article>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string EditerName { get; set; }

        public int Status { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateTime { get; set; }

        public byte[] Timestamp { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
