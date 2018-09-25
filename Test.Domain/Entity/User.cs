using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Domain.Entity
{
    public class User
    {
        public User()
        {
            Articles = new HashSet<Article>();
        }
        public int Id { get; set; }

        public DateTime CreateTime { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public int Status { get; set; }

        public string Mobile { get; set; }

        public string MailBox { get; set; }

        public string SaltValue { get; set; }

        public byte[] Timestamp { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
