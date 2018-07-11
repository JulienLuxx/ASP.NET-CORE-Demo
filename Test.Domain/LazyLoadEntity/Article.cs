using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Test.Domain.LazyLoadEntity
{
    public class Article
    {
        private ILazyLoader LazyLoader { get; set; }
        public Article(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
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

        private ICollection<Comment> _comments;
        public ICollection<Comment> Comments
        {
            get => LazyLoader?.Load(this, ref _comments);
            set => _comments = value;
        }
    }
}
