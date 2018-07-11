using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Domain.Entity
{
    public class EntityBase
    {
        public EntityBase()
        {
            IsDelete = false;
        }
        public int Id { get; set; }

        public DateTime? CreateTime { get; set; }

        public bool IsDelete { get; set; }
    }
}
