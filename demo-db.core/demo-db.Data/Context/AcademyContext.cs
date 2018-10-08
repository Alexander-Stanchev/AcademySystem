﻿using demo_db.Data.DataModels;
using Microsoft.EntityFrameworkCore;


namespace demo_db.Data.Context
{
    public class AcademyContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Assaignment> Assaignments { get; set; }

        public DbSet<EnrolledStudent> EnrolledStudents { get; set; }

        public DbSet<Grade> Grades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.
                    UseSqlServer("Server=localhost;Database=Academy;Trusted_Connection=True;");
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnrolledStudent>()
                .HasKey(e => new { e.CourseId, e.StudentId });

            modelBuilder.Entity<EnrolledStudent>()
                .HasOne(es => es.Student)
                .WithMany(s => s.EnrolledStudents)
                .HasForeignKey(es => es.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EnrolledStudent>()
                .HasOne(es => es.Course)
                .WithMany(c => c.EnrolledStudents)
                .HasForeignKey(es => es.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Grade>()
                .HasKey(g => new { g.AssaignmentId, g.StudentId });

            modelBuilder.Entity<Grade>()
                .HasOne(gr => gr.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(gr => gr.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Grade>()
                .HasOne(gr => gr.Assaignment)
                .WithMany(a => a.Grades)
                .HasForeignKey(g => g.AssaignmentId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}