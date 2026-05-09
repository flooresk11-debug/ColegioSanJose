using Microsoft.EntityFrameworkCore;
using ColegioSanJose.Models.DB;

namespace ColegioSanJose.Data
{
    public class ColegioContext : DbContext
    {
        public ColegioContext(DbContextOptions<ColegioContext> options)
            : base(options) { }

        public DbSet<Alumno> Alumnos { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Expediente> Expedientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expediente>()
                .Property(e => e.NotaFinal)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<Expediente>()
                .HasOne(e => e.Alumno)
                .WithMany(a => a.Expedientes)
                .HasForeignKey(e => e.AlumnoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Expediente>()
                .HasOne(e => e.Materia)
                .WithMany(m => m.Expedientes)
                .HasForeignKey(e => e.MateriaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Alumno>().ToTable("Alumno");
            modelBuilder.Entity<Materia>().ToTable("Materia");
            modelBuilder.Entity<Expediente>().ToTable("Expediente");

            base.OnModelCreating(modelBuilder);
        }
    }
}