using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.DTOs
{
    public class TaskItemDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
