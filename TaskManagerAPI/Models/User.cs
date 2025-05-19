using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public byte [] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        public List<TaskItem> Tasks { get; set; }
    }
}
