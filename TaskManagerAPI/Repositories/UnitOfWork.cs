using TaskManagerAPI.Data;

namespace TaskManagerAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ITaskRepository Tasks { get; }

        public UnitOfWork(AppDbContext context, ITaskRepository taskRepository)
        {
            _context = context;
            Tasks = taskRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
