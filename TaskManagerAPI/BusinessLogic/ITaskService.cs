namespace TaskManagerAPI.BusinessLogic;

using TaskManagerAPI.Models;
using TaskManagerAPI.DTOs;


public interface ITaskService
{
    Task<IEnumerable<TaskItemDTO>> GetAllTasksAsync();
    Task<TaskItemDTO?> GetTaskByIdAsync(int id);
    Task<TaskItemDTO> CreateTaskAsync(TaskItemDTO taskDto);
    Task<bool> UpdateTaskAsync(int id, TaskItemDTO taskDto);
    Task<bool> DeleteTaskAsync(int id);
}
