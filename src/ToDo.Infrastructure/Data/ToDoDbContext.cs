using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities;

namespace ToDo.Infrastructure.Data
{
    /// <summary>
    /// DbContext para a aplicação ToDo
    /// Responsável por configurar as entidades e mapear para o banco de dados
    /// </summary>
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DbSet para tarefas
        /// </summary>
        public DbSet<TodoTask> Tasks { get; set; }

        /// <summary>
        /// Configurações do modelo (mapeamento para banco de dados)
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar tabela Tasks
            modelBuilder.Entity<TodoTask>(entity =>
            {
                entity.ToTable("Tasks");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .IsRequired(false);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.CompletedAt)
                    .IsRequired(false);

                entity.Property(e => e.Status)
                    .HasConversion<int>();
            });
        }
    }
}
