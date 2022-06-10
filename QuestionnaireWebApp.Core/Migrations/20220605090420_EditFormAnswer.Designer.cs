﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using QuestionnaireWebApp.Core;

#nullable disable

namespace QuestinaryWebApp.Core.Migrations
{
    [DbContext(typeof(QuestionnaireContext))]
    [Migration("20220605090420_EditFormAnswer")]
    partial class EditFormAnswer
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("QuestionnaireWebApp.Core.Models.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ExtraQuestionId")
                        .HasColumnType("integer");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<int>("QuestionId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ExtraQuestionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("QuestionnaireWebApp.Core.Models.Form", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("PersonId")
                        .HasColumnType("integer");

                    b.Property<int>("QuestinnaryId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("QuestinnaryId");

                    b.ToTable("Forms");
                });

            modelBuilder.Entity("QuestionnaireWebApp.Core.Models.FormAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("AnswerId")
                        .HasColumnType("integer");

                    b.Property<int>("FormId")
                        .HasColumnType("integer");

                    b.Property<string>("OurAnswer")
                        .HasColumnType("text");

                    b.Property<int>("QuestionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("FormId");

                    b.HasIndex("QuestionId");

                    b.ToTable("FormAnswers");
                });

            modelBuilder.Entity("QuestionnaireWebApp.Core.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Sex")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("QuestionnaireWebApp.Core.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<int>("QuestionnaireeId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("QuestionnaireeId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("QuestionnaireWebApp.Core.Models.Questionnaire", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Questionnairees");
                });

            modelBuilder.Entity("QuestionnaireWebApp.Core.Models.Answer", b =>
                {
                    b.HasOne("QuestionnaireWebApp.Core.Models.Question", "ExtraQuestion")
                        .WithMany()
                        .HasForeignKey("ExtraQuestionId");

                    b.HasOne("QuestionnaireWebApp.Core.Models.Question", "Question")
                        .WithMany("AnswerVariants")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExtraQuestion");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("QuestionnaireWebApp.Core.Models.Form", b =>
                {
                    b.HasOne("QuestionnaireWebApp.Core.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuestionnaireWebApp.Core.Models.Questionnaire", "Questionnaire")
                        .WithMany()
                        .HasForeignKey("QuestinnaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Questionnaire");
                });

            modelBuilder.Entity("QuestionnaireWebApp.Core.Models.FormAnswer", b =>
                {
                    b.HasOne("QuestionnaireWebApp.Core.Models.Answer", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId");

                    b.HasOne("QuestionnaireWebApp.Core.Models.Form", "Form")
                        .WithMany()
                        .HasForeignKey("FormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuestionnaireWebApp.Core.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("Form");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("QuestionnaireWebApp.Core.Models.Question", b =>
                {
                    b.HasOne("QuestionnaireWebApp.Core.Models.Questionnaire", "Questionnaire")
                        .WithMany("Questions")
                        .HasForeignKey("QuestionnaireeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Questionnaire");
                });

            modelBuilder.Entity("QuestionnaireWebApp.Core.Models.Question", b =>
                {
                    b.Navigation("AnswerVariants");
                });

            modelBuilder.Entity("QuestionnaireWebApp.Core.Models.Questionnaire", b =>
                {
                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}