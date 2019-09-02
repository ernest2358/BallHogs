﻿// <auto-generated />
using System;
using BallHogs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BallHogs.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20190901164132_AddCustRost")]
    partial class AddCustRost
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BallHogs.Models.BHTeam", b =>
                {
                    b.Property<int>("BHTeamId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ManagerID");

                    b.Property<string>("ManagerName");

                    b.Property<string>("TeamName");

                    b.HasKey("BHTeamId");

                    b.HasIndex("ManagerID");

                    b.ToTable("BHTeams");
                });

            modelBuilder.Entity("BallHogs.Models.Datum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("First_name");

                    b.Property<int?>("Height_feet");

                    b.Property<int?>("Height_inches");

                    b.Property<string>("Last_name");

                    b.Property<string>("Position");

                    b.Property<int?>("TeamId");

                    b.Property<int?>("Weight_pounds");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Datum");
                });

            modelBuilder.Entity("BallHogs.Models.Manager", b =>
                {
                    b.Property<int>("ManagerID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Losses");

                    b.Property<string>("UserName");

                    b.Property<int>("Wins");

                    b.HasKey("ManagerID");

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("BallHogs.Models.ManagerTeam", b =>
                {
                    b.Property<int>("ManagerTeamId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BHTeamId");

                    b.Property<int>("ManagerId");

                    b.HasKey("ManagerTeamId");

                    b.HasIndex("BHTeamId");

                    b.HasIndex("ManagerId");

                    b.ToTable("ManagerTeams");
                });

            modelBuilder.Entity("BallHogs.Models.PlayersOnTeams", b =>
                {
                    b.Property<int>("PlayersOnTeamsId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BHTeamId");

                    b.Property<int>("DatumId");

                    b.HasKey("PlayersOnTeamsId");

                    b.HasIndex("BHTeamId");

                    b.HasIndex("DatumId");

                    b.ToTable("PlayersOnTeams");
                });

            modelBuilder.Entity("BallHogs.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Abbreviation");

                    b.Property<string>("City");

                    b.Property<string>("Conference");

                    b.Property<string>("Division");

                    b.Property<string>("Full_name");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("BallHogs.Models.BHTeam", b =>
                {
                    b.HasOne("BallHogs.Models.Manager")
                        .WithMany("BHTeams")
                        .HasForeignKey("ManagerID");
                });

            modelBuilder.Entity("BallHogs.Models.Datum", b =>
                {
                    b.HasOne("BallHogs.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("BallHogs.Models.ManagerTeam", b =>
                {
                    b.HasOne("BallHogs.Models.BHTeam", "BHTeam")
                        .WithMany()
                        .HasForeignKey("BHTeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BallHogs.Models.Manager", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BallHogs.Models.PlayersOnTeams", b =>
                {
                    b.HasOne("BallHogs.Models.BHTeam", "BHTeam")
                        .WithMany("Players")
                        .HasForeignKey("BHTeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BallHogs.Models.Datum", "Datum")
                        .WithMany()
                        .HasForeignKey("DatumId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}