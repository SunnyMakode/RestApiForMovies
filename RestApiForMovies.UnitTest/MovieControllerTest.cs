using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestApiForMovies.BusinessLogic.DataTransferObjects;
using RestApiForMovies.BusinessLogic.Entities;
using RestApiForMovies.BusinessLogic.ServiceInterface;
using RestApiForMovies.Common;
using RestApiForMovies.Controllers;
using RestApiForMovies.DataPersistance;

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
        IDataService<Movie> _service;
        MoviesController _controller;

        [OneTimeSetUp]
        public void Setup()
        {
            //Arrange
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

        [Test]
        public async Task CallingGetFilteredMovies_WithFakeInterface_ExpectingNotImplementedException()
        {
            try
            {
                var query = new QueryFilter { MovieName = "Avengers: End Game" };
                var result = await _controller.GetFilteredMovies(query).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Assert.That(e.Message, Is.EqualTo("The method or operation is not implemented."));
            }
        }

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
                testMovieData.Add(DataMapper.MappingDtoToMovie(movieItem));
            }

            testDbContext.Movies.AddRange(testMovieData);
            testDbContext.SaveChanges();
        }        
    }
}