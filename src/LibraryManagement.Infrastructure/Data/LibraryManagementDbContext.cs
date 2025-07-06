using LibraryManagement.Domain.Authors;
using LibraryManagement.Domain.Books;
using LibraryManagement.Domain.Borrowers;
using LibraryManagement.Domain.Loans;
using LibraryManagement.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LibraryManagement.Infrastructure.Data;

public class LibraryManagementDbContext(DbContextOptions<LibraryManagementDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Borrower> Borrowers { get; set; }
    public DbSet<Loan> Loans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasOne<Author>()
            .WithMany()
            .HasForeignKey(e => e.AuthorId);

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasOne<Borrower>()
                .WithMany()
                .HasForeignKey(e => e.BorrowerId);

            entity.HasOne<Book>()
                .WithMany()
                .HasForeignKey(e => e.BookId);
        });

        base.OnModelCreating(modelBuilder);
    }
}
