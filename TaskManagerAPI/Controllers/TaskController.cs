using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagerAPI.BusinessLogic;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Middleware.Exceptions;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>Получить список всех задач</summary>
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        /// <summary>Получить конкретную задачу</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) throw new NotFoundException("Задача не найдена");
            return Ok(task);
        }

        /// <summary>Создать задачу</summary>
        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskItemDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _taskService.CreateTaskAsync(dto);
            return CreatedAtAction(nameof(GetTask), new { id = created.Id }, created);
        }

        /// <summary>Обновить задачу</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItemDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _taskService.UpdateTaskAsync(id, dto);
            if (!updated) return NotFound();

            return NoContent();
        }

        /// <summary>Удалить задачу</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var deleted = await _taskService.DeleteTaskAsync(id);
            if (!deleted) return NotFound();

            return NoContent(); // стандартный ответ для DELETE
        }
    }
}
