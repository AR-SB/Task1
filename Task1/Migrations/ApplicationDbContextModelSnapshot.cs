﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Task1.Data;

#nullable disable

namespace Task1.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Task1.Models.Admin", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"));

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EmployeeId");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("Task1.Models.Application", b =>
                {
                    b.Property<int>("ApplicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ApplicationId"));

                    b.Property<DateTime?>("ActionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("ApplicationStatus")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Major")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SubmissionTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("TelephoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ApplicationId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("Task1.Models.Application", b =>
                {
                    b.HasOne("Task1.Models.Admin", null)
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
