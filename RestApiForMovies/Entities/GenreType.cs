using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestApiForMovies.Entities
{
    public class GenreType
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }

        //[JsonIgnore]
        //public Movie Movie { get; set; } //one movie can have many genre 1:N
        //[JsonIgnore]
        //public int MovieId { get; set; }

        [JsonIgnore]
        public ICollection<Movie_Genre> MovieGenres { get; set; }
    }
}