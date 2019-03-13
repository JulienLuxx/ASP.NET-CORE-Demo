﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Test.Domain;

namespace Test.Domain.Migrations
{
    [DbContext(typeof(TestDBContext))]
    [Migration("20190311083447_initDB")]
    partial class initDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Test.Domain.Entity.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("Status");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Title");

                    b.Property<int>("TypeId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Article");
                });

            modelBuilder.Entity("Test.Domain.Entity.ArticleType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("EditerName");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<int>("Status");

                    b.Property<uint>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnName("xmin")
                        .HasColumnType("xid");

                    b.HasKey("Id");

                    b.ToTable("ArticleType");
                });

            modelBuilder.Entity("Test.Domain.Entity.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

                    b.Property<int?>("ArticleId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Creator");

                    b.Property<bool>("IsDelete");

                    b.Property<int?>("ParentId");

                    b.Property<int>("Status");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("Test.Domain.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("MailBox");

                    b.Property<string>("Mobile");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("SaltValue");

                    b.Property<int>("Status");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Test.Domain.Entity.Article", b =>
                {
                    b.HasOne("Test.Domain.Entity.ArticleType", "ArticleType")
                        .WithMany("Articles")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Test.Domain.Entity.User", "User")
                        .WithMany("Articles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Test.Domain.Entity.Comment", b =>
                {
                    b.HasOne("Test.Domain.Entity.Article", "Article")
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId");
                });
#pragma warning restore 612, 618
        }
    }
}