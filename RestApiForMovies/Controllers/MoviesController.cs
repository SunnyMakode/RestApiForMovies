using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApiForMovies.DataPersistance;
using RestApiForMovies.DataPersistance.DataService;
using RestApiForMovies.DataTransferObjects;
using RestApiForMovies.Entities;

namespace RestApiForMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService<Movie> _movieService;
        private readonly DataContext _context;

        public MoviesController(IMovieService<Movie> movieService, DataContext context)
        {
            this._movieService = movieService;
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movies = await _movieService.GetAll();

            List<MovieDto> movieDtos = new List<MovieDto>();
            foreach (var movieItem in movies)
            {
                movieDtos.Add(new MovieDto
                {
                    Id = movieItem.Id,
                    Name = movieItem.Name,
                    Year = movieItem.Year,
                    AgeLimit = movieItem.AgeLimit,
                    Rating = movieItem.Rating,
                    Synopsis = movieItem.Synopsis,
                    Genres = GetGenreFromDomainObject(movieItem.MovieGenres),
                    Actors = GetActorFromDomainObject(movieItem.MovieActors),
                    Director = new DirectorDto 
                    { 
                        FirstName = movieItem.Director.FirstName,
                        LastName = movieItem.Director.LastName
                    }
                });
            }

            return movieDtos;
        }        

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _movieService.Get(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _movieService.Update(movie);
            //_context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies        
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(MovieDto movieDto)
        {            
            Movie movie = new Movie
            {
                Name = movieDto.Name,
                Year = movieDto.Year,
                AgeLimit = movieDto.AgeLimit,
                Rating = movieDto.Rating,
                Synopsis = movieDto.Synopsis,
                Director = new Director
                {
                    FirstName = movieDto.Director.FirstName,
                    LastName = movieDto.Director.LastName
                },
                MovieGenres = GetGenreFromDataTranferObject(movieDto.Genres),
                MovieActors = GetActorFromDataTransferObject(movieDto.Actors)
            };

            this._movieService.Add(movie);
            //_context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await this._movieService.Get(id);

            if (movie == null)
            {
                return NotFound();
            }

            this._movieService.Remove(movie);
            //_context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        private List<ActorDto> GetActorFromDomainObject(ICollection<Movie_Actor> movieActors)
        {
            if (movieActors == null)
                return null;

            var actorsDto = new List<ActorDto>();
            foreach (var movieActorItem in movieActors)
            {
                actorsDto.Add(new ActorDto
                {
                    FirstName = movieActorItem.Actor.FirstName,
                    LastName = movieActorItem.Actor.LastName
                });
            }
            return actorsDto;
        }

        private List<string> GetGenreFromDomainObject(ICollection<Movie_Genre> movieGenres)
        {
            if (movieGenres == null)
                return null;

            var genresDto = new List<string>();
            foreach (var movieGenreItem in movieGenres)
            {
                genresDto.Add(movieGenreItem.Genre.Name);
            }
            return genresDto;
        }

        private ICollection<Movie_Actor> GetActorFromDataTransferObject(List<ActorDto> actors)
        {
            var actorList = new List<Movie_Actor>();

            foreach (var actor in actors)
            {
                actorList.Add(new Movie_Actor
                {
                    Actor = new Actor
                    {
                        FirstName = actor.FirstName,
                        LastName = actor.LastName,
                    }
                });
            }
            return actorList;
        }

        private ICollection<Movie_Genre> GetGenreFromDataTranferObject(List<string> genres)
        {

            var genreList = new List<Movie_Genre>();

            foreach (var genreName in genres)
            {
                genreList.Add(new Movie_Genre
                {
                    Genre = new GenreType { Name = genreName }
                });
            }
            return genreList;
        }
    }
}
