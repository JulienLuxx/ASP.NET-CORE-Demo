using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Test.Domain.LazyLoadEntity
{
    public class Comment 
    {
        private ILazyLoader LazyLoader { get; set; }
        public Comment(ILazyLoader lazyLoader)
        {
            State = 0;
            IsDelete = false;
            LazyLoader = lazyLoader;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDelete { get; set; }

        [StringLength(200)]
        public string Creator { get; set; }

        [StringLength(2000)]
        public string Content { get; set; }

        public int ParentId { get; set; }

        public int State { get; set; }

        public int? ArticleId { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        private Article _article;

        [ForeignKey("ArticleId")]
        public Article Article
        {
            get => LazyLoader?.Load(this, ref _article);
            set => _article = value;
        }
    }
}
