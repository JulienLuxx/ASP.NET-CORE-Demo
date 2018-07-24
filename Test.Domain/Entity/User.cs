using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Domain.Entity
{
    public class User
    {
        public int Id { get; set; }

        public DateTime CreateTime { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public int Status { get; set; }
    }
}
