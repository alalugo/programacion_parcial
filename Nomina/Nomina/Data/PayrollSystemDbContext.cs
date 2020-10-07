using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Nomina.Entities;

namespace Nomina.Data
{
    public partial class PayrollSystemDbContext : DbContext
    {
        public PayrollSystemDbContext()
        {
        }

        public PayrollSystemDbContext(DbContextOptions<PayrollSystemDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Departamentos> Departamentos { get; set; }
        public virtual DbSet<Empleados> Empleados { get; set; }
        public virtual DbSet<Puestos> Puestos { get; set; }
        public virtual DbSet<TipoIngreso> TipoIngreso { get; set; }
        public virtual DbSet<TiposDeducciones> TiposDeducciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Departamentos>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UbicacionFisica)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Empleados>(entity =>
            {
                entity.Property(e => e.Cedula)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SalarioMensual)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0.00))");

                entity.HasOne(d => d.IdDepartamentoNavigation)
                    .WithMany(p => p.Empleados)
                    .HasForeignKey(d => d.IdDepartamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Empleados__IdDep__286302EC");

                entity.HasOne(d => d.IdPuestoNavigation)
                    .WithMany(p => p.Empleados)
                    .HasForeignKey(d => d.IdPuesto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Empleados__IdPue__29572725");
            });

            modelBuilder.Entity<Puestos>(entity =>
            {
                entity.Property(e => e.NivelMaximoSalario).HasColumnType("money");

                entity.Property(e => e.NivelMinimoSalario).HasColumnType("money");

                entity.Property(e => e.NivelRiesgo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoIngreso>(entity =>
            {
                entity.Property(e => e.DependeSalario)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposDeducciones>(entity =>
            {
                entity.Property(e => e.DependeSalario)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
