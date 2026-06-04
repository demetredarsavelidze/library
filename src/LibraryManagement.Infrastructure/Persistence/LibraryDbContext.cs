using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence;

public sealed class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();

    public DbSet<Author> Authors => Set<Author>();

    public DbSet<Genre> Genres => Set<Genre>();

    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(book => book.Title).HasMaxLength(200).IsRequired();
            entity.Property(book => book.Pages).IsRequired();
            entity.Property(book => book.PublicationYear).IsRequired();

            entity.HasOne(book => book.Author)
                .WithMany(author => author.Books)
                .HasForeignKey(book => book.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(book => book.Genre)
                .WithMany(genre => genre.Books)
                .HasForeignKey(book => book.GenreId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(author => author.FullName).HasMaxLength(200).IsRequired();
            entity.Property(author => author.BirthDate).IsRequired();
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.Property(genre => genre.Name).HasMaxLength(100).IsRequired();
            entity.HasIndex(genre => genre.Name).IsUnique();
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(user => user.UserName).HasMaxLength(100).IsRequired();
            entity.Property(user => user.PasswordHash).IsRequired();
            entity.Property(user => user.Role).HasMaxLength(20).IsRequired();
            entity.HasIndex(user => user.UserName).IsUnique();
        });
    }
}
