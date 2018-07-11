using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Domain.Entity;

namespace Test.Domain.Config
{
    public class ArticleConfig: IEntityTypeConfig
    {
        public ArticleConfig(ModelBuilder builder)
        {
            builder.Entity<Article>().ToTable("Article");
        }
    }
}
