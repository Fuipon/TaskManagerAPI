using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagerAPI.BusinessLogic
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            return await _unitOfWork.Tasks.GetAllAsync();
        }

        public async Task<TaskItem> GetTaskByIdAsync(int id)
        {
            return await _unitOfWork.Tasks.GetByIdAsync(id);
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem task)
        {
            await _unitOfWork.Tasks.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();
            return task;
        }

        public async Task<bool> UpdateTaskAsync(int id, TaskItem task)
        {
            var existingTask = await _unitOfWork.Tasks.GetByIdAsync(id);

            if (existingTask == null)
                return false;

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.IsCompleted = task.IsCompleted;

            _unitOfWork.Tasks.Update(existingTask);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var existingTask = await _unitOfWork.Tasks.GetByIdAsync(id);

            if (existingTask == null)
                return false;

            _unitOfWork.Tasks.Delete(existingTask);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
