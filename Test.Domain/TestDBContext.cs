﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
//using Test.Domain.Entity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Test.Domain.Entity;
using System.Data;

namespace Test.Domain
{
    public class TestDBContext : DbContext
    {
        //public TestDBContext(DbContextOptions<TestDBContext> options) : base(options)
        //{ }

        public TestDBContext() { }

        public virtual DbSet<Article> Article { get; set; }

        public virtual DbSet<ArticleType> ArticleType { get; set; }

        public virtual DbSet<Comment> Comment { get; set; }

        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //UseLazyLoadProxies,ConfigureIgnoreDetachLazyLoadingWarning
            //optionsBuilder.UseLazyLoadingProxies().ConfigureWarnings(action => action.Ignore(CoreEventId.DetachedLazyLoadingWarning)).UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TestDB;Trusted_Connection=True;");
            optionsBuilder.UseLazyLoadingProxies().ConfigureWarnings(action => action.Ignore(CoreEventId.DetachedLazyLoadingWarning)).UseSqlServer(@"Data Source=192.168.3.165;Initial Catalog=TestDB;User ID=sa;Password=zzz;", x => x.UseRowNumberForPaging());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region OldVersionMethod
            //var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            //.Where(type => !String.IsNullOrEmpty(type.Namespace))
            //.Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));
            //foreach (var type in typesToRegister)
            //{
            //    dynamic configurationInstance = Activator.CreateInstance(type);
            //    //modelBuilder.Configurations.Add(configurationInstance);
            //}
            #endregion

            #region TestMethod
            //var entityTypes = Assembly.GetExecutingAssembly().GetTypes()
            //    .Where(type => !string.IsNullOrWhiteSpace(type.Name))
            //    .Where(type => type.GetTypeInfo().IsClass)
            //    .Where(type => type.GetTypeInfo().BaseType != null)
            //    .Where(type => typeof(IEntity).IsAssignableFrom(type)).ToList();

            //foreach (var item in entityTypes)
            //{
            //    if(modelBuilder.Model.FindEntityType)
            //}
            #endregion

            #region Fluent API
            #region Sample
            //modelBuilder.Entity<Article>(e =>
            //{
            //    e.ToTable("Article");
            //    e.HasKey(x => x.Id);
            //    e.OwnsOne(x => x.Comments);
            //});
            //modelBuilder.Entity<Article>().ToTable("Article").HasKey(x => x.Id);
            ////PositiveWay
            ////modelBuilder.Entity<Article>().HasMany(x => x.Comments).WithOne(y => y.Article).HasForeignKey(y => y.ArticleId); 
            //modelBuilder.Entity<Comment>().ToTable("Comment").HasKey(x => x.Id);
            ////ReverseWay
            //modelBuilder.Entity<Comment>().HasOne(x => x.Article).WithMany(y => y.Comments).HasForeignKey(x => x.ArticleId);
            #endregion

            modelBuilder.Entity<Article>(e =>
            {
                e.ToTable("Article");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                //e.Ignore(x => x.Timestamp);
                e.Property(x => x.Timestamp).IsRowVersion().IsConcurrencyToken();
                e.HasOne(x => x.User).WithMany(y => y.Articles).HasForeignKey(x => x.UserId);
            });

            modelBuilder.Entity<ArticleType>(e =>
            {
                e.ToTable("ArticleType");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                //e.Ignore(x => x.Timestamp);
                e.Property(x => x.Timestamp).IsRowVersion().IsConcurrencyToken();
                e.HasMany(x => x.Articles).WithOne(y => y.ArticleType).HasForeignKey(y => y.TypeId);
            });

            modelBuilder.Entity<Comment>(e =>
            {
                e.ToTable("Comment");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                //e.Ignore(x => x.Timestamp);
                e.Property(x => x.Timestamp).IsRowVersion().IsConcurrencyToken();
                e.HasOne(x => x.Article).WithMany(y => y.Comments).HasForeignKey(x => x.ArticleId);
            });

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                //e.Ignore(x => x.Timestamp);
                e.Property(x => x.Timestamp).IsRowVersion().IsConcurrencyToken();
                e.HasMany(x => x.Articles).WithOne(y => y.User).HasForeignKey(y => y.UserId);
            });
            #endregion
        }

    }
}
