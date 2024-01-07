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
    [Migration("20240107015129_AddColorAndDisplayOrderColumns")]
    partial class AddColorAndDisplayOrderColumns
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WarcraftGearPlanner.Server.Data.Entities.InventoryTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int?>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Type", "Name")
                        .IsUnique();

                    b.ToTable("InventoryTypes");
                });

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

                    b.Property<int?>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClassId")
                        .IsUnique();

                    b.ToTable("ItemClasses");
                });

            modelBuilder.Entity("WarcraftGearPlanner.Server.Data.Entities.ItemEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<Guid?>("InventoryTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsEquippable")
                        .HasColumnType("bit");

                    b.Property<bool>("IsStackable")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ItemClassId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("ItemId")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("ItemQualityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ItemSubclassId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Level")
                        .HasColumnType("bigint");

                    b.Property<long>("MaxCount")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("PurchasePrice")
                        .HasColumnType("bigint");

                    b.Property<long>("PurchaseQuantity")
                        .HasColumnType("bigint");

                    b.Property<long>("RequiredLevel")
                        .HasColumnType("bigint");

                    b.Property<long>("SellPrice")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("InventoryTypeId");

                    b.HasIndex("ItemClassId");

                    b.HasIndex("ItemQualityId");

                    b.HasIndex("ItemSubclassId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("WarcraftGearPlanner.Server.Data.Entities.ItemQualityEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int?>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Type", "Name")
                        .IsUnique();

                    b.ToTable("ItemQualities");
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

                    b.Property<int?>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<bool>("HideTooltip")
                        .HasColumnType("bit");

                    b.Property<Guid>("ItemClassId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubclassId")
                        .HasColumnType("int");

                    b.Property<string>("VerboseName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ItemClassId", "SubclassId")
                        .IsUnique();

                    b.ToTable("ItemSubclasses");
                });

            modelBuilder.Entity("WarcraftGearPlanner.Server.Data.Entities.ItemSubclassInventoryTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<Guid>("InventoryTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ItemSubclassId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("InventoryTypeId");

                    b.HasIndex("ItemSubclassId");

                    b.ToTable("ItemSubclassInventoryTypeEntity");
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

                    b.Property<bool>("IsInaccessible")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RealmId")
                        .HasColumnType("int");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Realms");
                });

            modelBuilder.Entity("WarcraftGearPlanner.Server.Data.Entities.ItemEntity", b =>
                {
                    b.HasOne("WarcraftGearPlanner.Server.Data.Entities.InventoryTypeEntity", "InventoryType")
                        .WithMany("Items")
                        .HasForeignKey("InventoryTypeId");

                    b.HasOne("WarcraftGearPlanner.Server.Data.Entities.ItemClassEntity", "ItemClass")
                        .WithMany("Items")
                        .HasForeignKey("ItemClassId");

                    b.HasOne("WarcraftGearPlanner.Server.Data.Entities.ItemQualityEntity", "ItemQuality")
                        .WithMany("Items")
                        .HasForeignKey("ItemQualityId");

                    b.HasOne("WarcraftGearPlanner.Server.Data.Entities.ItemSubclassEntity", "ItemSubclass")
                        .WithMany("Items")
                        .HasForeignKey("ItemSubclassId");

                    b.Navigation("InventoryType");

                    b.Navigation("ItemClass");

                    b.Navigation("ItemQuality");

                    b.Navigation("ItemSubclass");
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

            modelBuilder.Entity("WarcraftGearPlanner.Server.Data.Entities.ItemSubclassInventoryTypeEntity", b =>
                {
                    b.HasOne("WarcraftGearPlanner.Server.Data.Entities.InventoryTypeEntity", "InventoryType")
                        .WithMany("ItemSubclassInventoryTypes")
                        .HasForeignKey("InventoryTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WarcraftGearPlanner.Server.Data.Entities.ItemSubclassEntity", "ItemSubclass")
                        .WithMany("ItemSubclassInventoryTypes")
                        .HasForeignKey("ItemSubclassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InventoryType");

                    b.Navigation("ItemSubclass");
                });

            modelBuilder.Entity("WarcraftGearPlanner.Server.Data.Entities.InventoryTypeEntity", b =>
                {
                    b.Navigation("ItemSubclassInventoryTypes");

                    b.Navigation("Items");
                });

            modelBuilder.Entity("WarcraftGearPlanner.Server.Data.Entities.ItemClassEntity", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("Subclasses");
                });

            modelBuilder.Entity("WarcraftGearPlanner.Server.Data.Entities.ItemQualityEntity", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("WarcraftGearPlanner.Server.Data.Entities.ItemSubclassEntity", b =>
                {
                    b.Navigation("ItemSubclassInventoryTypes");

                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
