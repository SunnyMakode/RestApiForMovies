using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestApiForMovies.BusinessLogic.Entities
{
    public class Movie
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public ICollection<Movie_Genre> MovieGenres { get; set; }
        public byte AgeLimit { get; set; }
        public byte Rating { get; set; }
        public ICollection<Movie_Actor> MovieActors { get; set; }
        public Director Director { get; set; }
        public string Synopsis { get; set; }
    }
}
