using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
//using Test.Domain.Entity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Test.Domain.Entity;
using System.Data;
using Microsoft.Extensions.Options;

namespace Test.Domain
{
    public class TestDBContext : DbContext
    {
        private DbContextOptions<TestDBContext> _options;
        public TestDBContext(DbContextOptions<TestDBContext> options) : base(options)
        {
            _options = options;
        }

        public TestDBContext()
        { }

        public virtual DbSet<Article> Article { get; set; }

        public virtual DbSet<ArticleType> ArticleType { get; set; }

        public virtual DbSet<Comment> Comment { get; set; }

        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<Log> Log { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(null==_options)
            {
                optionsBuilder.UseMySql(@"server=47.244.228.240;port=3266;database=TestDB;user=root;password=2134006;TreatTinyAsBoolean=True");
            }
            base.OnConfiguring(optionsBuilder);
            //    //UseLazyLoadProxies,ConfigureIgnoreDetachLazyLoadingWarning
            //    //MSSql
            //    //optionsBuilder.UseLazyLoadingProxies().ConfigureWarnings(action => action.Ignore(CoreEventId.DetachedLazyLoadingWarning)).UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TestDB;Trusted_Connection=True;");
            //    //MySql
            //    //RemberToAdd(TreatTinyAsBoolean=True)ForCompatibility
            //    optionsBuilder.UseMySql(@"server=localhost;database=TestDB;user=root;password=2134006;TreatTinyAsBoolean=True");
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
                e.Property(x => x.Id).ValueGeneratedOnAdd().UseMySqlIdentityColumn();//MySqlSetIncrement(Verify)
 
                e.Property(x => x.Title).HasMaxLength(256);

                e.Property(x => x.Timestamp).IsRowVersion().IsConcurrencyToken();                
            });

            modelBuilder.Entity<ArticleType>(e =>
            {
                e.ToTable("ArticleType");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd().UseMySqlIdentityColumn();//MySqlSetIncrement(Verify)
                e.Property(x => x.Timestamp).IsRowVersion().IsConcurrencyToken();

                e.Property(x => x.Name).HasMaxLength(512);
                e.Property(x => x.EditerName).HasMaxLength(512);

                e.HasMany(x => x.Articles).WithOne(y => y.ArticleType).HasForeignKey(y => y.TypeId);
            });

            modelBuilder.Entity<Comment>(e =>
            {
                e.ToTable("Comment");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd().UseMySqlIdentityColumn();//MySqlSetIncrement(Verify)
                e.Property(x => x.Timestamp).IsRowVersion().IsConcurrencyToken();

                e.Property(x => x.Creator).HasMaxLength(256);

                e.HasOne(x => x.Article).WithMany(y => y.Comments).HasForeignKey(x => x.ArticleId);
            });

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd().UseMySqlIdentityColumn();//MySqlSetIncrement(Verify)
                e.Property(x => x.Timestamp).IsRowVersion().IsConcurrencyToken();

                e.Property(x => x.Name).HasMaxLength(256);
                e.Property(x => x.Password).HasMaxLength(256);
                e.Property(x => x.Mobile).HasMaxLength(128);
                e.Property(x => x.MailBox).HasMaxLength(256);
                e.Property(x => x.SaltValue).HasMaxLength(256);

                e.HasMany(x => x.Articles).WithOne(y => y.User).HasForeignKey(y => y.UserId);
            });

            modelBuilder.Entity<Log>(e =>
            {
                e.ToTable("Log");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd().UseMySqlIdentityColumn();

                e.Property(x => x.Application).HasMaxLength(64);
                e.Property(x => x.Level).HasMaxLength(64);
                e.Property(x => x.Message).HasMaxLength(512);
                e.Property(x => x.Logger).HasMaxLength(256);
                e.Property(x => x.Callsite).HasMaxLength(512);
                e.Property(x => x.Exception).HasMaxLength(512);
            });
            #endregion
        }

    }
}
