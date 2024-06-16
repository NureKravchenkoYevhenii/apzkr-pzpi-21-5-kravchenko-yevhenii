﻿// <auto-generated />
using System;
using DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(ParkyDbContext))]
    [Migration("20240508080428_ModelChanges")]
    partial class ModelChanges
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BookDate")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("ExpiresIn")
                        .HasColumnType("time");

                    b.Property<int>("ParkingPlaceId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParkingPlaceId");

                    b.HasIndex("UserId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("Domain.Models.Membership", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DurationInTimeUnits")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("TimeUnitValue")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Membership", t =>
                        {
                            t.HasCheckConstraint("CHK_DurationInTimeUnits_Memberships", "[DurationInTimeUnits] >= 0");

                            t.HasCheckConstraint("CHK_Price_Memberships", "[Price] >= 0");
                        });
                });

            modelBuilder.Entity("Domain.Models.ParkingPlace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ParkingPlaces");
                });

            modelBuilder.Entity("Domain.Models.ParkingSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ParkingPlaceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParkingPlaceId");

                    b.HasIndex("UserId");

                    b.ToTable("ParkingSessions", t =>
                        {
                            t.HasCheckConstraint("CHK_EndDate_ParkingSessions", "[EndDate] > [StartDate]");
                        });
                });

            modelBuilder.Entity("Domain.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PayDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Sum")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("Transaction")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Payments", t =>
                        {
                            t.HasCheckConstraint("CHK_Sum_Payments", "[Sum] > 0");
                        });
                });

            modelBuilder.Entity("Domain.Models.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ExpiresOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Token")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Domain.Models.ResetPasswordToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ExpiresOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ResetPasswordToken");
                });

            modelBuilder.Entity("Domain.Models.Tarif", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ActiveOnDaysOfWeek")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PricePerTimeUnit")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time");

                    b.Property<int>("TimeUnitValue")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tarif", t =>
                        {
                            t.HasCheckConstraint("CHK_EndTime_Tarifs", "[EndTime] > [StartTime]");

                            t.HasCheckConstraint("CHK_PricePerTimeUnit_Tarifs", "[PricePerTimeUnit] >= 0");
                        });
                });

            modelBuilder.Entity("Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("bit");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsBlocked = false,
                            Login = "Admin",
                            PasswordHash = "$2a$10$Usqo0Y9zHoU0Wpt5jZl2B.DKMaKZ7cT/tDDLTv9vwsj7PlaWs40Ai",
                            PasswordSalt = "G+94*Wzt@_e!GOFQ",
                            RegistrationDate = new DateTime(2024, 5, 8, 8, 4, 27, 727, DateTimeKind.Utc).AddTicks(4960),
                            Role = 1
                        });
                });

            modelBuilder.Entity("Domain.Models.UserMembership", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("bit");

                    b.Property<int>("MembershipId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MembershipId");

                    b.HasIndex("UserId");

                    b.ToTable("UserMembership");
                });

            modelBuilder.Entity("Domain.Models.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ProfilePicture")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.ToTable("UserProfiles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "Street Ave. 15",
                            BirthDate = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "admin@bibliworm.com",
                            FirstName = "Admin",
                            LastName = "Admin",
                            PhoneNumber = "0555555555",
                            ProfilePicture = new byte[0]
                        });
                });

            modelBuilder.Entity("Domain.Models.Booking", b =>
                {
                    b.HasOne("Domain.Models.ParkingPlace", "ParkingPlace")
                        .WithMany("Bookings")
                        .HasForeignKey("ParkingPlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.User", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParkingPlace");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Models.ParkingSession", b =>
                {
                    b.HasOne("Domain.Models.ParkingPlace", "ParkingPlace")
                        .WithMany("ParkingSessions")
                        .HasForeignKey("ParkingPlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.User", "User")
                        .WithMany("ParkingSessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParkingPlace");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Models.Payment", b =>
                {
                    b.HasOne("Domain.Models.User", "User")
                        .WithMany("Payments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Models.RefreshToken", b =>
                {
                    b.HasOne("Domain.Models.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Models.ResetPasswordToken", b =>
                {
                    b.HasOne("Domain.Models.User", "User")
                        .WithMany("ResetPasswordTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Models.UserMembership", b =>
                {
                    b.HasOne("Domain.Models.Membership", "Membership")
                        .WithMany("UserMemberships")
                        .HasForeignKey("MembershipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.User", "User")
                        .WithMany("UserMemberships")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Membership");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Models.UserProfile", b =>
                {
                    b.HasOne("Domain.Models.User", "User")
                        .WithOne("UserProfile")
                        .HasForeignKey("Domain.Models.UserProfile", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Models.Membership", b =>
                {
                    b.Navigation("UserMemberships");
                });

            modelBuilder.Entity("Domain.Models.ParkingPlace", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("ParkingSessions");
                });

            modelBuilder.Entity("Domain.Models.User", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("ParkingSessions");

                    b.Navigation("Payments");

                    b.Navigation("RefreshTokens");

                    b.Navigation("ResetPasswordTokens");

                    b.Navigation("UserMemberships");

                    b.Navigation("UserProfile");
                });
#pragma warning restore 612, 618
        }
    }
}
