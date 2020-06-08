﻿// <auto-generated />
using System;
using EveryBus.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EveryBus.Migrations
{
    [DbContext(typeof(BusContext))]
    partial class BusContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("EveryBus.Domain.Models.Route", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Destination")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<Guid?>("ServiceId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ServiceId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("EveryBus.Domain.Models.Service", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Color")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ServiceType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("TextColor")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("EveryBus.Domain.Models.Stop", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("AtocCode")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Direction")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Identifier")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<string>("Locality")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Orientation")
                        .HasColumnType("int");

                    b.Property<Guid?>("RouteId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ServiceType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("StopId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.ToTable("Stops");
                });

            modelBuilder.Entity("EveryBus.Domain.Models.VehicleLocation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Destination")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("Heading")
                        .HasColumnType("int");

                    b.Property<string>("JourneyId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("LastGpsFix")
                        .HasColumnType("int");

                    b.Property<double>("Latitude")
                        .HasColumnType("double");

                    b.Property<double>("Longitude")
                        .HasColumnType("double");

                    b.Property<string>("NextStopId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ServiceName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("Speed")
                        .HasColumnType("int");

                    b.Property<string>("VehicleId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("VehicleType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("LastGpsFix");

                    b.ToTable("VehicleLocations");
                });

            modelBuilder.Entity("EveryBus.Domain.Models.Route", b =>
                {
                    b.HasOne("EveryBus.Domain.Models.Service", null)
                        .WithMany("Routes")
                        .HasForeignKey("ServiceId");
                });

            modelBuilder.Entity("EveryBus.Domain.Models.Stop", b =>
                {
                    b.HasOne("EveryBus.Domain.Models.Route", null)
                        .WithMany("Stops")
                        .HasForeignKey("RouteId");
                });
#pragma warning restore 612, 618
        }
    }
}
