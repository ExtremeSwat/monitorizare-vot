﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VoteMonitor.Entities;

namespace VoteMonitor.Entities.Migrations
{
    [DbContext(typeof(VoteMonitorContext))]
    [Migration("20190928123414_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VoteMonitor.Entities.Answer", b =>
                {
                    b.Property<int>("IdObserver");

                    b.Property<int>("IdOptionToQuestion");

                    b.Property<int>("IdPollingStation");

                    b.Property<string>("CountyCode")
                        .HasMaxLength(2);

                    b.Property<DateTime>("LastModified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("PollingStationNumber");

                    b.Property<string>("Value")
                        .HasMaxLength(1000);

                    b.HasKey("IdObserver", "IdOptionToQuestion", "IdPollingStation")
                        .HasName("PK_Answer");

                    b.HasIndex("IdObserver")
                        .HasName("IX_Answer_IdObserver");

                    b.HasIndex("IdOptionToQuestion")
                        .HasName("IX_Answer_IdOptionToQuestion");

                    b.HasIndex("IdPollingStation")
                        .HasName("IX_Answer_IdPollingStation");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("VoteMonitor.Entities.County", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("NumberOfPollingStations");

                    b.HasKey("Id")
                        .HasName("PK_County");

                    b.ToTable("Counties");
                });

            modelBuilder.Entity("VoteMonitor.Entities.Form", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(2)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(2);

                    b.Property<int>("CurrentVersion");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.HasKey("Id")
                        .HasName("PK_FormVersion");

                    b.ToTable("Forms");
                });

            modelBuilder.Entity("VoteMonitor.Entities.FormSection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(4);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<int>("IdForm");

                    b.HasKey("Id")
                        .HasName("PK_FormSection");

                    b.HasIndex("IdForm");

                    b.ToTable("FormSections");
                });

            modelBuilder.Entity("VoteMonitor.Entities.Ngo", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<bool>("Organizer")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("Id")
                        .HasName("PK_NGO");

                    b.ToTable("Ngos");
                });

            modelBuilder.Entity("VoteMonitor.Entities.NgoAdmin", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("IdNgo");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id")
                        .HasName("PK_NgoAdminId");

                    b.HasIndex("IdNgo");

                    b.ToTable("NgoAdmin");
                });

            modelBuilder.Entity("VoteMonitor.Entities.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdObserver");

                    b.Property<int>("IdPollingStation");

                    b.Property<int?>("IdQuestion");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime");

                    b.Property<string>("Text");

                    b.HasKey("Id")
                        .HasName("PK_Note");

                    b.HasIndex("IdObserver")
                        .HasName("IX_Note_IdObserver");

                    b.HasIndex("IdPollingStation")
                        .HasName("IX_Note_IdPollingStation");

                    b.HasIndex("IdQuestion")
                        .HasName("IX_Note_IdQuestion");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("VoteMonitor.Entities.NoteAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("NoteId");

                    b.Property<string>("NotePath")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.HasKey("Id")
                        .HasName("PK_NoteAttachment");

                    b.HasIndex("NoteId");

                    b.ToTable("NoteAttachments");
                });

            modelBuilder.Entity("VoteMonitor.Entities.Observer", b =>
                {
                    b.Property<int>("Id");

                    b.Property<DateTime?>("DeviceRegisterDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("FromTeam")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<int>("IdNgo");

                    b.Property<string>("MobileDeviceId")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Pin")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id")
                        .HasName("PK_Observer");

                    b.HasIndex("IdNgo")
                        .HasName("IX_Observer_IdNgo");

                    b.ToTable("Observers");
                });

            modelBuilder.Entity("VoteMonitor.Entities.Option", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Hint");

                    b.Property<bool>("IsFreeText")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.HasKey("Id")
                        .HasName("PK_Option");

                    b.ToTable("Options");
                });

            modelBuilder.Entity("VoteMonitor.Entities.OptionToQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Flagged")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<int>("IdOption");

                    b.Property<int>("IdQuestion");

                    b.HasKey("Id")
                        .HasName("PK_OptionToQuestion");

                    b.HasIndex("IdOption")
                        .HasName("IX_OptionToQuestion_Option");

                    b.HasIndex("IdQuestion")
                        .HasName("IX_OptionToQuestion_Question");

                    b.HasIndex("IdOption", "IdQuestion")
                        .IsUnique()
                        .HasName("IX_OptionToQuestion");

                    b.ToTable("OptionsToQuestions");
                });

            modelBuilder.Entity("VoteMonitor.Entities.PollingStation", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Address")
                        .HasMaxLength(500);

                    b.Property<string>("AdministrativeTerritoryCode")
                        .HasMaxLength(100);

                    b.Property<string>("Coordinates")
                        .HasColumnType("varchar(200)");

                    b.Property<int>("IdCounty");

                    b.Property<int>("Number");

                    b.Property<string>("TerritoryCode")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id")
                        .HasName("PK_PollingStation");

                    b.HasIndex("IdCounty")
                        .HasName("IX_PollingStation_IdCounty");

                    b.HasIndex("IdCounty", "Id")
                        .IsUnique()
                        .HasName("IX_Unique_IdCounty_IdPollingStation");

                    b.ToTable("PollingStations");
                });

            modelBuilder.Entity("VoteMonitor.Entities.PollingStationInfo", b =>
                {
                    b.Property<int>("IdObserver");

                    b.Property<int>("IdPollingStation");

                    b.Property<bool?>("IsPollingStationPresidentFemale");

                    b.Property<DateTime>("LastModified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("getdate()");

                    b.Property<DateTime?>("ObserverArrivalTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("ObserverLeaveTime")
                        .HasColumnType("datetime");

                    b.Property<bool?>("UrbanArea");

                    b.HasKey("IdObserver", "IdPollingStation")
                        .HasName("PK_PollingStationInfo");

                    b.HasIndex("IdObserver")
                        .HasName("IX_PollingStationInfo_IdObserver");

                    b.HasIndex("IdPollingStation")
                        .HasName("IX_PollingStationInfo_IdPollingStation");

                    b.ToTable("PollingStationInfos");
                });

            modelBuilder.Entity("VoteMonitor.Entities.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<string>("Hint");

                    b.Property<int>("IdSection");

                    b.Property<int>("QuestionType");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id")
                        .HasName("PK_Question");

                    b.HasIndex("IdSection")
                        .HasName("IX_Question_IdSection");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("VoteMonitor.Entities.Answer", b =>
                {
                    b.HasOne("VoteMonitor.Entities.Observer", "Observer")
                        .WithMany("Answers")
                        .HasForeignKey("IdObserver")
                        .HasConstraintName("FK_Answer_Observer")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("VoteMonitor.Entities.OptionToQuestion", "OptionAnswered")
                        .WithMany("Answers")
                        .HasForeignKey("IdOptionToQuestion")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("VoteMonitor.Entities.PollingStation", "PollingStation")
                        .WithMany("Answers")
                        .HasForeignKey("IdPollingStation")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("VoteMonitor.Entities.FormSection", b =>
                {
                    b.HasOne("VoteMonitor.Entities.Form", "Form")
                        .WithMany("FormSections")
                        .HasForeignKey("IdForm")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VoteMonitor.Entities.NgoAdmin", b =>
                {
                    b.HasOne("VoteMonitor.Entities.Ngo", "Ngo")
                        .WithMany("NgoAdmins")
                        .HasForeignKey("IdNgo")
                        .HasConstraintName("FK_NgoAdmin_Ngo")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("VoteMonitor.Entities.Note", b =>
                {
                    b.HasOne("VoteMonitor.Entities.Observer", "Observer")
                        .WithMany("Notes")
                        .HasForeignKey("IdObserver")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("VoteMonitor.Entities.PollingStation", "PollingStation")
                        .WithMany("Notes")
                        .HasForeignKey("IdPollingStation")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("VoteMonitor.Entities.Question", "Question")
                        .WithMany("Notes")
                        .HasForeignKey("IdQuestion")
                        .HasConstraintName("FK_Note_Question");
                });

            modelBuilder.Entity("VoteMonitor.Entities.NoteAttachment", b =>
                {
                    b.HasOne("VoteMonitor.Entities.Note", "Note")
                        .WithMany("NoteAttachments")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VoteMonitor.Entities.Observer", b =>
                {
                    b.HasOne("VoteMonitor.Entities.Ngo", "Ngo")
                        .WithMany("Observers")
                        .HasForeignKey("IdNgo")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("VoteMonitor.Entities.OptionToQuestion", b =>
                {
                    b.HasOne("VoteMonitor.Entities.Option", "Option")
                        .WithMany("OptionsToQuestions")
                        .HasForeignKey("IdOption")
                        .HasConstraintName("FK_OptionToQuestion_Option")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("VoteMonitor.Entities.Question", "Question")
                        .WithMany("OptionsToQuestions")
                        .HasForeignKey("IdQuestion")
                        .HasConstraintName("FK_OptionToQuestion_Question")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("VoteMonitor.Entities.PollingStation", b =>
                {
                    b.HasOne("VoteMonitor.Entities.County", "County")
                        .WithMany("PollingStations")
                        .HasForeignKey("IdCounty")
                        .HasConstraintName("FK_PollingStation_County")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("VoteMonitor.Entities.PollingStationInfo", b =>
                {
                    b.HasOne("VoteMonitor.Entities.Observer", "Observer")
                        .WithMany("PollingStationInfos")
                        .HasForeignKey("IdObserver")
                        .HasConstraintName("FK_PollingStationInfo_Observer")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("VoteMonitor.Entities.PollingStation", "PollingStation")
                        .WithMany("PollingStationInfos")
                        .HasForeignKey("IdPollingStation")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("VoteMonitor.Entities.Question", b =>
                {
                    b.HasOne("VoteMonitor.Entities.FormSection", "FormSection")
                        .WithMany("Questions")
                        .HasForeignKey("IdSection")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
