using Microsoft.EntityFrameworkCore;
using SchoolEquipmentApi.Models;

namespace SchoolEquipmentApi.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            // Check if already seeded
            if (await context.Users.AnyAsync())
                return;

            // Seed Users with default password: "password123"
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("password123");
            
            var users = new List<User>
            {
                new User { Login = "admin", PasswordHash = passwordHash, Role = "Admin", FullName = "Администратор" },
                new User { Login = "operator", PasswordHash = passwordHash, Role = "Operator", FullName = "Оператор" },
                new User { Login = "viewer", PasswordHash = passwordHash, Role = "Viewer", FullName = "Просмотрщик" }
            };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
    }
}