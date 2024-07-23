using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CowManagerApp.MVC.Models;

public partial class CowManagerContext : DbContext
{
    public CowManagerContext()
    {
    }

    public CowManagerContext(DbContextOptions<CowManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cow> Cows { get; set; }

    public virtual DbSet<Diagnosis> Diagnoses { get; set; }

    public virtual DbSet<Disease> Diseases { get; set; }

    public virtual DbSet<Herd> Herds { get; set; }

    public virtual DbSet<Medicine> Medicines { get; set; }

    public virtual DbSet<Treatment> Treatments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CowManager;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cow>(entity =>
        {
            entity.ToTable("Cow");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Comment)
                .HasMaxLength(200)
                .IsFixedLength();
            entity.Property(e => e.Idherd).HasColumnName("IDHerd");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsFixedLength();

            entity.HasOne(d => d.IdherdNavigation).WithMany(p => p.Cows)
                .HasForeignKey(d => d.Idherd)
                .HasConstraintName("FK_Cow_Herd");
        });

        modelBuilder.Entity<Diagnosis>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Diagnosis");

            entity.Property(e => e.Comment)
                .HasMaxLength(200)
                .IsFixedLength();
            entity.Property(e => e.Idcow).HasColumnName("IDCow");
            entity.Property(e => e.Iddisease).HasColumnName("IDDisease");
            entity.Property(e => e.NameOfDisease)
                .HasMaxLength(50)
                .IsFixedLength();

            entity.HasOne(d => d.IdcowNavigation).WithMany()
                .HasForeignKey(d => d.Idcow)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Diagnosis_Cow");

            entity.HasOne(d => d.IddiseaseNavigation).WithMany()
                .HasForeignKey(d => d.Iddisease)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Diagnosis_Disease");
        });

        modelBuilder.Entity<Disease>(entity =>
        {
            entity.ToTable("Disease");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Comment)
                .HasMaxLength(200)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        modelBuilder.Entity<Herd>(entity =>
        {
            entity.ToTable("Herd");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Comment)
                .HasMaxLength(200)
                .IsFixedLength();
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.ToTable("Medicine");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Comment)
                .HasMaxLength(200)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        modelBuilder.Entity<Treatment>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Treatment");

            entity.Property(e => e.Comment)
                .HasMaxLength(200)
                .IsFixedLength();
            entity.Property(e => e.Idcow).HasColumnName("IDCow");
            entity.Property(e => e.Idmedicine).HasColumnName("IDMedicine");
            entity.Property(e => e.NameOfMedicine)
                .HasMaxLength(50)
                .IsFixedLength();

            entity.HasOne(d => d.IdcowNavigation).WithMany()
                .HasForeignKey(d => d.Idcow)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Treatment_Cow");

            entity.HasOne(d => d.IdmedicineNavigation).WithMany()
                .HasForeignKey(d => d.Idmedicine)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Treatment_Medicine");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
