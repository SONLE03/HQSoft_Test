using HQSoft_EX01.DTOs.Response;
using HQSoft_EX01.Models;
using Microsoft.EntityFrameworkCore;

namespace HQSoft_EX01.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {

        }
        public DbSet<Books> Books { get; set; }
        public DbSet<Authors> Authors { get; set; }
        public DbSet<BooksWithAuthors> BooksWithAuthors { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Authors>().HasData(
            new Authors { AuthorId = 1, Name = "John Doe", Bio = "A prolific author in programming." },
            new Authors { AuthorId = 2, Name = "Jane Smith", Bio = "Specializes in fiction." }
        );

            modelBuilder.Entity<Books>().HasData(
                new Books { BookId = 1, Title = "Learn C#", Price = 29.99m, PublishedDate = new DateTime(2023, 5, 1), AuthorId = 1 },
                new Books { BookId = 2, Title = "Mastering ASP.NET", Price = 39.99m, PublishedDate = new DateTime(2023, 8, 1), AuthorId = 1 },
                new Books { BookId = 3, Title = "Fictional Worlds", Price = 19.99m, PublishedDate = new DateTime(2023, 7, 15), AuthorId = 2 }
            );
            modelBuilder.Entity<BooksWithAuthors>()
                .HasNoKey();
            modelBuilder.Entity<Books>()
               .HasOne(b => b.Author)
               .WithMany()  
               .HasForeignKey(b => b.AuthorId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
