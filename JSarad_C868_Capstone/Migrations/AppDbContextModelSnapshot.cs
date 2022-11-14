﻿// <auto-generated />
using System;
using JSarad_C868_Capstone.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JSarad_C868_Capstone.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("JSarad_C868_Capstone.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Clients");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "123 Country Road",
                            Email = "eledford@email.com",
                            LastUpdate = new DateTime(2022, 11, 13, 16, 57, 17, 709, DateTimeKind.Local).AddTicks(250),
                            Name = "Edwin Ledford",
                            Phone = "6613332222"
                        });
                });

            modelBuilder.Entity("JSarad_C868_Capstone.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Availability")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "2414 Loma Linda Dr",
                            Availability = "MTWRFSU",
                            Email = "jsarad2@wgu.edu",
                            LastUpdate = new DateTime(2022, 11, 13, 16, 57, 17, 709, DateTimeKind.Local).AddTicks(284),
                            Name = "Johanna Sarad",
                            Phone = "6614444763",
                            Role = "Bartender"
                        },
                        new
                        {
                            Id = 2,
                            Address = "345 Mullberry Way",
                            Availability = "TRFSU",
                            Email = "rcrocker@email.com",
                            LastUpdate = new DateTime(2022, 11, 13, 16, 57, 17, 709, DateTimeKind.Local).AddTicks(287),
                            Name = "Rebecca Crocker",
                            Phone = "6613332211",
                            Role = "Server"
                        },
                        new
                        {
                            Id = 3,
                            Address = "765 Atlantic St",
                            Availability = "MWF",
                            Email = "iward@email.com",
                            LastUpdate = new DateTime(2022, 11, 13, 16, 57, 17, 709, DateTimeKind.Local).AddTicks(289),
                            Name = "Ian Ward",
                            Phone = "8057778899",
                            Role = "Server"
                        });
                });

            modelBuilder.Entity("JSarad_C868_Capstone.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Bar")
                        .HasColumnType("bit");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Food")
                        .HasColumnType("bit");

                    b.Property<int>("Guests")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Events");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Bar = true,
                            ClientId = 1,
                            CreatedBy = 1,
                            CreatedOn = new DateTime(2022, 10, 21, 16, 30, 0, 0, DateTimeKind.Unspecified),
                            EndTime = new DateTime(2022, 11, 10, 20, 0, 0, 0, DateTimeKind.Unspecified),
                            EventDate = new DateTime(2022, 11, 10, 16, 0, 0, 0, DateTimeKind.Unspecified),
                            EventName = "Ledford LLC. Luncheon",
                            Food = true,
                            Guests = 50,
                            LastUpdate = new DateTime(2022, 11, 13, 16, 57, 17, 709, DateTimeKind.Local).AddTicks(300),
                            Location = "888 Corporate Way",
                            Notes = "",
                            StartTime = new DateTime(2022, 11, 10, 16, 0, 0, 0, DateTimeKind.Unspecified),
                            Type = "Corporate"
                        });
                });

            modelBuilder.Entity("JSarad_C868_Capstone.Models.EventSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<int>("ScheduleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("EventSchedules");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            EventId = 1,
                            ScheduleId = 1
                        },
                        new
                        {
                            Id = 2,
                            EventId = 1,
                            ScheduleId = 2
                        },
                        new
                        {
                            Id = 3,
                            EventId = 1,
                            ScheduleId = 3
                        });
                });

            modelBuilder.Entity("JSarad_C868_Capstone.Models.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Schedules");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            EmployeeId = 1,
                            EndTime = new DateTime(2022, 11, 10, 10, 0, 0, 0, DateTimeKind.Unspecified),
                            StartTime = new DateTime(2022, 11, 10, 4, 30, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            EmployeeId = 2,
                            EndTime = new DateTime(2022, 11, 10, 10, 0, 0, 0, DateTimeKind.Unspecified),
                            StartTime = new DateTime(2022, 11, 10, 4, 30, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("JSarad_C868_Capstone.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Password = "Test",
                            Username = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            Password = "password",
                            Username = "Planner"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
