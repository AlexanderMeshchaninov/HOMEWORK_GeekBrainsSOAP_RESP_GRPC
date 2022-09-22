﻿// <auto-generated />
using System;
using ClinicService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClinicService.Data.Migrations
{
    [DbContext(typeof(ClinicServiceDbContext))]
    partial class ClinicServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ClinicService.Data.Models.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AccountId"));

                    b.Property<string>("EMail")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<bool>("Locked")
                        .HasColumnType("boolean");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("ClinicService.Data.Models.AccountSession", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SessionId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("boolean");

                    b.Property<string>("SessionToken")
                        .IsRequired()
                        .HasMaxLength(384)
                        .HasColumnType("character varying(384)");

                    b.Property<DateTime?>("TimeClosed")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("TimeCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("TimeLastRequest")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("SessionId");

                    b.HasIndex("AccountId");

                    b.ToTable("AccountSessions");
                });

            modelBuilder.Entity("ClinicService.Data.Models.Client", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ClientId"));

                    b.Property<string>("Document")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Patronymic")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Surname")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("ClientId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("ClinicService.Data.Models.Consultation", b =>
                {
                    b.Property<int>("ConsultationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ConsultationId"));

                    b.Property<int>("ClientId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ConsultationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("PetId")
                        .HasColumnType("integer");

                    b.HasKey("ConsultationId");

                    b.HasIndex("ClientId");

                    b.HasIndex("PetId");

                    b.ToTable("Consultations");
                });

            modelBuilder.Entity("ClinicService.Data.Models.Pet", b =>
                {
                    b.Property<int>("PetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PetId"));

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ClientId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("PetId");

                    b.HasIndex("ClientId");

                    b.ToTable("Pets");
                });

            modelBuilder.Entity("ClinicService.Data.Models.AccountSession", b =>
                {
                    b.HasOne("ClinicService.Data.Models.Account", "Account")
                        .WithMany("Sessions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ClinicService.Data.Models.Consultation", b =>
                {
                    b.HasOne("ClinicService.Data.Models.Client", "Client")
                        .WithMany("Consultations")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicService.Data.Models.Pet", "Pet")
                        .WithMany("Consultations")
                        .HasForeignKey("PetId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Pet");
                });

            modelBuilder.Entity("ClinicService.Data.Models.Pet", b =>
                {
                    b.HasOne("ClinicService.Data.Models.Client", "Client")
                        .WithMany("Pets")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("ClinicService.Data.Models.Account", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("ClinicService.Data.Models.Client", b =>
                {
                    b.Navigation("Consultations");

                    b.Navigation("Pets");
                });

            modelBuilder.Entity("ClinicService.Data.Models.Pet", b =>
                {
                    b.Navigation("Consultations");
                });
#pragma warning restore 612, 618
        }
    }
}
