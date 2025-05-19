namespace TaskManagerAPI.Repositories
{
    public interface IUnitOfWork
    {
        ITaskRepository Tasks { get; }
        Task<int> SaveChangesAsync();
    }
}