using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApiForMovies.BusinessLogic.DataTransferObjects;
using RestApiForMovies.BusinessLogic.Entities;
using RestApiForMovies.BusinessLogic.ServiceInterface;
using RestApiForMovies.Common;
using RestApiForMovies.DataPersistance;
using System.Linq.Expressions;

namespace RestApiForMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IDataService<Movie> _movieService;
        private readonly DataContext _context;

        public MoviesController(IDataService<Movie> movieService,
                                DataContext context)
        {
            this._movieService = movieService;
            this._context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<IEnumerable<MovieDto>> GetMovies()
        {
            var movies = await _movieService.GetAll();
            var movieDtos = DataMapper.MappingMovieToDto(movies);
            return movieDtos;
        }        

        // GET: api/Movies/query
        [HttpGet("query")]
        public async Task<IEnumerable<MovieDto>> GetFilteredMovies([FromQuery] QueryFilter queryFilter)
        {
            var movies = await _movieService.GetAll(CreateQuery(queryFilter));
            var movieDtos = DataMapper.MappingMovieToDto(movies);
            return movieDtos;
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await _movieService.Get(id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDtos = DataMapper.MappingMovieToDto(new List<Movie> { movie });
            
            return movieDtos.FirstOrDefault();
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
            Movie movie = DataMapper.MappingDtoToMovie(movieDto);

            this._movieService.Add(movie);

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

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        private static Expression<Func<Movie, bool>> CreateQuery(QueryFilter queryFilter)
        {
            return x => x.Name == queryFilter.MovieName
                     || x.Director.FirstName == queryFilter.Director.FirstName
                     || x.AgeLimit == queryFilter.AgeLimit;
        }
    }
}
