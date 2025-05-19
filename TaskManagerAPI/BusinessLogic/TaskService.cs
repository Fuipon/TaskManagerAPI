using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;
using TaskManagerAPI.DTOs;

namespace TaskManagerAPI.BusinessLogic
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskItemDTO>> GetAllTasksAsync()
        {
            var tasks = await _unitOfWork.Tasks.GetAllAsync();
            return _mapper.Map<IEnumerable<TaskItemDTO>>(tasks);
        }

        public async Task<TaskItemDTO> GetTaskByIdAsync(int id)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            return _mapper.Map<TaskItemDTO?>(task); ;
        }

        public async Task<TaskItemDTO> CreateTaskAsync(TaskItemDTO dto)
        {
            var task = _mapper.Map<TaskItem>(dto);
            await _unitOfWork.Tasks.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TaskItemDTO>(task);
        }

        public async Task<bool> UpdateTaskAsync(int id, TaskItemDTO dto)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            if (task == null) return false;

            _mapper.Map(dto, task); // Обновляем поля task из dto
            _unitOfWork.Tasks.Update(task);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            if (task == null) return false;

            _unitOfWork.Tasks.Delete(task);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
