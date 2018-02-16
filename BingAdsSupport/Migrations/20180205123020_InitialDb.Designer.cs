﻿// <auto-generated />
using BingAdsSupport.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace BingAdsSupport.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20180205123020_InitialDb")]
    partial class InitialDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BingAdsSupport.Entity.ICMTicket", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Accuracy");

                    b.Property<string>("Feedback");

                    b.Property<int>("ResponseTime");

                    b.Property<int>("Solution");

                    b.Property<int>("SupportExperience");

                    b.Property<string>("TicketID");

                    b.HasKey("ID");

                    b.ToTable("ICMTickets");
                });
#pragma warning restore 612, 618
        }
    }
}
