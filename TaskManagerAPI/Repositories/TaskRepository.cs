using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        // Получить все задачи из базы данных
        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            // Здесь можно добавить AsNoTracking(), если задачи не планируется менять
            return await _context.Tasks.AsNoTracking().ToListAsync();
        }

        // Получить задачу по ID
        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        // Добавить новую задачу (не сохраняет изменения, сохранение - в UnitOfWork)
        public async Task AddAsync(TaskItem task)
        {
            await _context.Tasks.AddAsync(task);
        }

        // Обновить задачу
        public void Update(TaskItem task)
        {
            _context.Tasks.Update(task);
        }

        // Удалить задачу
        public void Delete(TaskItem task)
        {
            _context.Tasks.Remove(task);
        }
    }
}
