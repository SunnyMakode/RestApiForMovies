using Microsoft.EntityFrameworkCore;
using RestApiForMovies.BusinessLogic.Entities;

namespace RestApiForMovies.DataPersistance
{
    public class DataContext : DbContext
    {
        //Contructor with the dependency of DbContextOptions
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<GenreType> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Movie_Genre> MovieGenres { get; set; }
        public DbSet<Movie_Actor> MovieActors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasOne(s => s.Director)
                .WithOne(i => i.Movies)
                .HasForeignKey<Director>(d => d.MovieId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Movie>()
                .Navigation(x => x.MovieGenres)
                .AutoInclude();

            modelBuilder.Entity<Movie>()
                .Navigation(x => x.MovieActors)
                .AutoInclude();

            modelBuilder.Entity<Movie_Actor>()
                .Navigation(x => x.Actor)
                .AutoInclude();

            modelBuilder.Entity<Movie_Genre>()
                .Navigation(x => x.Genre)
                .AutoInclude();

            modelBuilder.Entity<Movie>()
                .Navigation(x => x.Director)
                .AutoInclude();
                
        }                
    }
}
