using System.Threading.Tasks;
using ToDo.Domain.Entities;

namespace ToDo.Domain.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TodoTask>> GetAllAsync();
        Task<TodoTask?> GetByIdAsync(int id);
        Task AddAsync(TodoTask task);
        Task UpdateAsync(TodoTask task);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
