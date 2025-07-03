using LibraryManagement.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Data;

public class LibraryManagementDbContext(DbContextOptions<LibraryManagementDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var passwordHasher = new PasswordHasher<User>();
        var hashedPassword = passwordHasher.HashPassword(null!, "1234");
        var user = new User(
            id: Guid.NewGuid(),
            username: "Mohammad",
            email: "mohammad@test.com",
            password: hashedPassword,
            phoneNumber: "07988998998");

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasData([user]);
        });

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
}
