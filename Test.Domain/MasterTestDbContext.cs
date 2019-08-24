using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Domain.Entity;

namespace Test.Domain
{
    public class MasterTestDBContext : DbContext
    {
        private DbContextOptions<MasterTestDBContext> _options;
        public MasterTestDBContext(DbContextOptions<MasterTestDBContext> options) : base(options)
        {
            _options = options;
        }

        public MasterTestDBContext() { }

        public virtual DbSet<Article> Article { get; set; }

        public virtual DbSet<ArticleType> ArticleType { get; set; }

        public virtual DbSet<Comment> Comment { get; set; }

        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<Log> Log { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (null == _options)
            {
                optionsBuilder.UseNpgsql(@"Host=47.244.228.240;Port=5233;Database=TestDB;Username=root;Password=2134006;");
            }
            base.OnConfiguring(optionsBuilder);
            //UseLazyLoadProxies,ConfigureIgnoreDetachLazyLoadingWarning
            //MSSql
            //optionsBuilder.UseLazyLoadingProxies().ConfigureWarnings(action => action.Ignore(CoreEventId.DetachedLazyLoadingWarning)).UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TestDB;Trusted_Connection=True;");
            //MySql
            //optionsBuilder.UseMySql(@"server=localhost;database=TestDB;user=root;password=1234;");
            //PostgreSql
            //optionsBuilder.UseNpgsql(@"Host=localhost;Port=5432;Database=TestDB;Username=postgres;Password=123456");
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

            var converter = new ValueConverter<byte[], long>(v => BitConverter.ToInt64(v, 0), v => BitConverter.GetBytes(v));

            modelBuilder.Entity<Article>(e =>
            {
                e.ToTable("Article");
                e.HasKey(x => x.Id);
                //e.Property(x => x.Id).ValueGeneratedOnAdd().UseMySqlIdentityColumn();//MySqlSetIncrement(Verify)
                e.Property(x => x.Id).ValueGeneratedOnAdd().UseNpgsqlIdentityAlwaysColumn();

                e.Property(x => x.Title).HasColumnType("varchar").HasMaxLength(256);

                e.Property(x => x.Timestamp).IsRowVersion().IsConcurrencyToken();
            });

            modelBuilder.Entity<ArticleType>(e =>
            {
                e.ToTable("ArticleType");
                e.HasKey(x => x.Id);
                //e.Property(x => x.Id).ValueGeneratedOnAdd().UseMySqlIdentityColumn();//MySqlSetIncrement(Verify)                
                e.Property(x => x.Id).ValueGeneratedOnAdd().UseNpgsqlIdentityAlwaysColumn();//PostgreSqlSetIncrement(Verify)  
                e.Property(x => x.Timestamp).HasColumnName("xmin").HasColumnType("xid").HasConversion(converter).IsRequired().IsRowVersion().IsConcurrencyToken().ValueGeneratedOnAddOrUpdate(); //ConcurrencyClick(Verify) 

                e.Property(x => x.Name).HasMaxLength(512);
                e.Property(x => x.EditerName).HasMaxLength(512);

                e.HasMany(x => x.Articles).WithOne(y => y.ArticleType).HasForeignKey(y => y.TypeId);
            });


            modelBuilder.Entity<Comment>(e =>
            {
                e.ToTable("Comment");
                e.HasKey(x => x.Id);
                //e.Property(x => x.Id).ValueGeneratedOnAdd().UseMySqlIdentityColumn();//MySqlSetIncrement(Verify)
                e.Property(x => x.Id).ValueGeneratedOnAdd().UseNpgsqlIdentityAlwaysColumn();
                e.Property(x => x.Timestamp).IsRowVersion().IsConcurrencyToken();

                e.Property(x => x.Creator).HasMaxLength(256);

                e.HasOne(x => x.Article).WithMany(y => y.Comments).HasForeignKey(x => x.ArticleId);
            });

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
                e.HasKey(x => x.Id);
                //e.Property(x => x.Id).ValueGeneratedOnAdd().UseMySqlIdentityColumn();//MySqlSetIncrement(Verify)
                e.Property(x => x.Id).ValueGeneratedOnAdd().UseNpgsqlIdentityAlwaysColumn();
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
                e.Property(x => x.Id).ValueGeneratedOnAdd().UseNpgsqlIdentityAlwaysColumn();
                e.Property(x => x.Application).HasMaxLength(64);
                e.Property(x => x.Logged).HasMaxLength(128);
                e.Property(x => x.Level).HasMaxLength(64);
                e.Property(x => x.Message).HasMaxLength(512);
                e.Property(x => x.Logger).HasMaxLength(256);
                e.Property(x => x.CallSite).HasMaxLength(512);
                e.Property(x => x.Exception).HasMaxLength(512);
            });
            #endregion
        }

    }
}
