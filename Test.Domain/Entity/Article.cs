using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Test.Domain.Entity
{
    [Table("Article")]
    public class Article:IEntity
    {    
        public Article()
        {
            Type = 0;
            State = 0;
            IsDeleted = false;
            Comments = new HashSet<Comment>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreateTime { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(10000)]
        public string Content { get; set; }

        [Required]
        public int Type { get; set; }

        public int State { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
