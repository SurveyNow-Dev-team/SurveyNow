﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Domain.Entities.Field", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("Domain.Entities.Occupation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<long?>("FieldId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Income")
                        .HasPrecision(11, 2)
                        .HasColumnType("decimal(11,2)");

                    b.Property<string>("PlaceOfWork")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)");

                    b.Property<long?>("PositionKey")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.HasIndex("PositionKey");

                    b.ToTable("Occupations");
                });

            modelBuilder.Entity("Domain.Entities.Position", b =>
                {
                    b.Property<long>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Key"), 1L, 1);

                    b.Property<long>("FieldId")
                        .HasColumnType("bigint");

                    b.HasKey("Key");

                    b.HasIndex("FieldId");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("CreatedDate")
                        .HasPrecision(2)
                        .HasColumnType("datetime2(2)");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasPrecision(2)
                        .HasColumnType("datetime2(2)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("GoogleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("LangKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasPrecision(2)
                        .HasColumnType("datetime2(2)");

                    b.Property<long?>("OccupationId")
                        .HasColumnType("bigint");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("Point")
                        .HasPrecision(6, 1)
                        .HasColumnType("decimal(6,1)");

                    b.Property<int?>("RelationshipStatus")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("GoogleId")
                        .IsUnique();

                    b.HasIndex("OccupationId");

                    b.HasIndex("PhoneNumber")
                        .IsUnique()
                        .HasFilter("[PhoneNumber] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Entities.Occupation", b =>
                {
                    b.HasOne("Domain.Entities.Field", "Field")
                        .WithMany()
                        .HasForeignKey("FieldId");

                    b.HasOne("Domain.Entities.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionKey");

                    b.Navigation("Field");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("Domain.Entities.Position", b =>
                {
                    b.HasOne("Domain.Entities.Field", "Field")
                        .WithMany("Positions")
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.HasOne("Domain.Entities.Occupation", "Occupation")
                        .WithMany()
                        .HasForeignKey("OccupationId");

                    b.Navigation("Occupation");
                });

            modelBuilder.Entity("Domain.Entities.Field", b =>
                {
                    b.Navigation("Positions");
                });
#pragma warning restore 612, 618
        }
    }
}
