using RestApiForMovies.BusinessLogic.DataTransferObjects;
using RestApiForMovies.BusinessLogic.Entities;

namespace RestApiForMovies.Common
{
    public static class DataMapper
    {
        /// <summary>
        /// Mapping domain object (Movie) properties to MovieDto
        /// </summary>
        /// <param name="movies"></param>
        /// <returns></returns>
        public static IEnumerable<MovieDto> MappingMovieToDto(IEnumerable<Movie> movies)
        {
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

        /// <summary>
        /// This method maps Data transfer object (MovieDto) properties to domain object (Movie)
        /// </summary>
        /// <param name="movieDto"></param>
        /// <returns></returns>
        public static Movie MappingDtoToMovie(MovieDto movieDto)
        {
            return new Movie
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
        }

        /// <summary>
        /// This method maps proprties of Movie_Actor i.e. DomainObject to ActorDto
        /// </summary>
        /// <param name="movieActors"></param>
        /// <returns></returns>
        private static List<ActorDto> GetActorFromDomainObject(ICollection<Movie_Actor> movieActors)
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

        /// <summary>
        /// This method maps proprties of Movie_Genre i.e. DomainObject to primitive List<string> type
        /// </summary>
        /// <param name="movieGenres"></param>
        /// <returns></returns>
        private static List<string> GetGenreFromDomainObject(ICollection<Movie_Genre> movieGenres)
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

        /// <summary>
        /// This method maps properties from ActorDto to domain properties of Movie_Actor
        /// </summary>
        /// <param name="actors"></param>
        /// <returns></returns>
        private static ICollection<Movie_Actor> GetActorFromDataTransferObject(List<ActorDto> actors)
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

        /// <summary>
        /// This method maps properties from List<string> to properties of domain object  i.e. Movie_Genre
        /// </summary>
        /// <param name="genres"></param>
        /// <returns></returns>
        private static ICollection<Movie_Genre> GetGenreFromDataTranferObject(List<string> genres)
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
