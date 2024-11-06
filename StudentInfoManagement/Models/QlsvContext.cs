using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StudentInfoManagement.Models;

public partial class QlsvContext : DbContext
{
    public QlsvContext() {}

    public QlsvContext(DbContextOptions<QlsvContext> options)
        : base(options) {}

    public virtual DbSet<StudentInfo> StudentInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=TDTPC;Initial Catalog=QLSV;Integrated Security=True;Trust Server Certificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StudentI__3214EC271BBC39BE");

            entity.ToTable("StudentInfo");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Birthday).HasColumnType("datetime");
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.StudentCode).HasMaxLength(10);
            entity.Property(e => e.StudentName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}