﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WarcraftGearPlanner.Server.Data;

#nullable disable

namespace WarcraftGearPlanner.Server.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231223211721_UpdateBaseEntity")]
    partial class UpdateBaseEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WarcraftGearPlanner.Server.Data.Entities.ItemClassEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ItemClasses");
                });

            modelBuilder.Entity("WarcraftGearPlanner.Server.Data.Entities.ItemSubclassEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<Guid>("ItemClassId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubclassId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ItemClassId");

                    b.ToTable("ItemSubclasses");
                });

            modelBuilder.Entity("WarcraftGearPlanner.Server.Data.Entities.RealmEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RealmId")
                        .HasColumnType("int");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Realms");
                });

            modelBuilder.Entity("WarcraftGearPlanner.Server.Data.Entities.ItemSubclassEntity", b =>
                {
                    b.HasOne("WarcraftGearPlanner.Server.Data.Entities.ItemClassEntity", "ItemClass")
                        .WithMany("Subclasses")
                        .HasForeignKey("ItemClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ItemClass");
                });

            modelBuilder.Entity("WarcraftGearPlanner.Server.Data.Entities.ItemClassEntity", b =>
                {
                    b.Navigation("Subclasses");
                });
#pragma warning restore 612, 618
        }
    }
}
