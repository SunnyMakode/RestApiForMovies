using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestApiForMovies.Controllers;
using RestApiForMovies.DataPersistance;
using RestApiForMovies.DataPersistance.DataService;
using RestApiForMovies.DataTransferObjects;
using RestApiForMovies.Entities;

namespace RestApiForMovies.UnitTest
{
    [TestFixture]
    public class MovieControllerTest
    {        
        private static DbContextOptions<DataContext> testDbContextOptions
            = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "movieTest")
            .Options;

        DataContext testDbContext;
        IMovieService<Movie> _service;
        MoviesController _controller;

        [OneTimeSetUp]
        public void Setup()
        {
            testDbContext = new DataContext(testDbContextOptions);
            testDbContext.Database.EnsureCreated();
            SeedTestData();

            _service = new FakeMovieService<Movie>(testDbContext);
            _controller = new MoviesController(_service, testDbContext);
        }

        [OneTimeTearDown]
        public void Clean()
        {
            testDbContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task CallingGetMovies_FromMovieController_ExpectingToReturnMovies()
        {
            //Act
            var result = await _controller.GetMovies().ConfigureAwait(false);

            //Assert
            Assert.That(result, Is.All.Not.Null);
            Assert.That(result, Is.TypeOf<List<MovieDto>>());
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task CallingGetMovie_WithId_FromMovieController_ExpectingToReturnSpecificMovie(int id)
        {
            //Act
            var result = ((ActionResult<Movie>)await _controller.GetMovie(id).ConfigureAwait(false)).Value;

            //Assert
            Assert.That(result?.Name, Is.Not.Empty);
            Assert.That(result, Is.TypeOf<Movie>());
        }

        //public async Task CallingGetMovies_WithSearchFilters_FromMovieController_ExpectingFilteredResult()
        //{
        //    //Act
        //    var result = await _controller.GetMovies().ConfigureAwait(false);

        //    //Assert
        //    Assert.That(result?.Name, Is.Not.Empty);
        //    Assert.That(result, Is.TypeOf<Movie>());
        //}

        private void SeedTestData()
        {
            var dataFile = @"ProjectDocument/movies-compact.json";
            var testMovieData = new List<Movie>();
            var testMovieDtoData = new List<MovieDto>();
            using (StreamReader stream = new StreamReader(dataFile))
            {
                string json = stream.ReadToEnd();
                testMovieDtoData = JsonConvert.DeserializeObject<List<MovieDto>>(json);
            }

            foreach (var movieItem in testMovieDtoData)
            {
                testMovieData.Add(new Movie
                {
                    Name = movieItem.Name,
                    Year = movieItem.Year,
                    MovieGenres = GetGenreFromDataTranferObject(movieItem.Genres),
                    MovieActors = GetActorFromDataTransferObject(movieItem.Actors),

                    Director = new Director
                    {
                        FirstName = movieItem.Director.FirstName,
                        LastName = movieItem.Director.LastName,
                    },
                    Rating = movieItem.Rating,
                    AgeLimit = movieItem.AgeLimit,
                    Synopsis = movieItem.Synopsis
                });
            }

            testDbContext.Movies.AddRange(testMovieData);
            testDbContext.SaveChanges();
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