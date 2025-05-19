using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;


namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TaskController(AppDbContext context)
        { 
            _context = context; 
        }

        /// <summary>   
        /// Получить список всех задач
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItemDTO>>> GetTasks()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var tasks = await _context.Tasks
                .Where(t => t.User.Id == userId)
                .Select(t => new TaskItemDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted
                })
                .ToListAsync();

            return tasks;
        }
        /// <summary>
        /// Получить конкретную задачу
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemDTO>> GetTask(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var task = await _context.Tasks
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id && t.User.Id == userId);

            if (task == null)
                return NotFound();

            return new TaskItemDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted
            };
        }

        /// <summary>
        /// Создать задачу
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TaskItemDTO>> CreateTask(TaskItemDTO dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return Unauthorized();

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
                CreatedAt = DateTime.UtcNow,
                User = user
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            dto.Id = task.Id;
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, dto);
        }

        /// <summary>
        /// Заменить задачу
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItemDTO dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var task = await _context.Tasks
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id && t.User.Id == userId);

            if (task == null)
                return NotFound();

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Удалить задачу
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var task = await _context.Tasks
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id && t.User.Id == userId);

            if (task == null)
                return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return null;

            if (int.TryParse(userIdClaim.Value, out int userId))
                return userId;

            return null;
        }

    }
}
