using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Domain.Entity
{
    public class ArticleType : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateTime { get; set; }

        public byte[] Timestamp { get ; set ; }
    }
}
