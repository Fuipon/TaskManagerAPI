using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Seeders
{
    public static class DbSeeder
    {
        public static void Seed(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();

            if (!db.Tasks.Any())
            {
                db.Tasks.Add(new TaskItem { Title = "Пример таска", IsCompleted = false });
                db.SaveChanges();
            }
        }
    }

}
