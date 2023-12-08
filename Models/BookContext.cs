using Microsoft.EntityFrameworkCore;
using System;

namespace BookReservationSystem.Models
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=BookReservationDb.db");

    }

    
}