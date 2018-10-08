﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using demo_db.Data.Context;

namespace demo_db.Data.Migrations
{
    [DbContext(typeof(AcademyContext))]
    partial class AcademyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("demo_db.Data.DataModels.Assaignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CourseId");

                    b.Property<DateTime>("DateTime");

                    b.Property<int>("MaxPoints");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Assaignments");
                });

            modelBuilder.Entity("demo_db.Data.DataModels.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("End");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("Start");

                    b.Property<int>("TeacherId");

                    b.HasKey("CourseId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("demo_db.Data.DataModels.EnrolledStudent", b =>
                {
                    b.Property<int>("CourseId");

                    b.Property<int>("StudentId");

                    b.HasKey("CourseId", "StudentId");

                    b.HasIndex("StudentId");

                    b.ToTable("EnrolledStudents");
                });

            modelBuilder.Entity("demo_db.Data.DataModels.Grade", b =>
                {
                    b.Property<int>("AssaignmentId");

                    b.Property<int>("StudentId");

                    b.Property<double>("ReceivedGrade");

                    b.HasKey("AssaignmentId", "StudentId");

                    b.HasIndex("StudentId");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("demo_db.Data.DataModels.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("demo_db.Data.DataModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Deleted");

                    b.Property<string>("FullName")
                        .IsRequired();

                    b.Property<int?>("MentorId");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<DateTime>("RegisteredOn");

                    b.Property<int>("RoleId");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("MentorId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("demo_db.Data.DataModels.Assaignment", b =>
                {
                    b.HasOne("demo_db.Data.DataModels.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("demo_db.Data.DataModels.Course", b =>
                {
                    b.HasOne("demo_db.Data.DataModels.User", "Teacher")
                        .WithMany("TaughtCourses")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("demo_db.Data.DataModels.EnrolledStudent", b =>
                {
                    b.HasOne("demo_db.Data.DataModels.Course", "Course")
                        .WithMany("EnrolledStudents")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("demo_db.Data.DataModels.User", "Student")
                        .WithMany("EnrolledStudents")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("demo_db.Data.DataModels.Grade", b =>
                {
                    b.HasOne("demo_db.Data.DataModels.Assaignment", "Assaignment")
                        .WithMany("Grades")
                        .HasForeignKey("AssaignmentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("demo_db.Data.DataModels.User", "Student")
                        .WithMany("Grades")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("demo_db.Data.DataModels.User", b =>
                {
                    b.HasOne("demo_db.Data.DataModels.User", "Mentor")
                        .WithMany()
                        .HasForeignKey("MentorId");

                    b.HasOne("demo_db.Data.DataModels.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
