using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities;
using ToDo.Domain.Repositories;
using ToDo.Infrastructure.Data;

namespace ToDo.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação do repositório de tarefas usando EF Core
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        private readonly ToDoDbContext _context;

        public TaskRepository(ToDoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoTask>> GetAllAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<TodoTask?> GetByIdAsync(int id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(TodoTask task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public async Task UpdateAsync(TodoTask task)
        {
            _context.Tasks.Update(task);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var task = await GetByIdAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
            }
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
