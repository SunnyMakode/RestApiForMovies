using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestApiForMovies.Entities
{
    public class Movie_Genre
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public int MovieId { get; set; }
        [JsonIgnore]
        public Movie Movie { get; set; }
        [JsonIgnore]
        public int GenreId { get; set; }
        public GenreType Genre { get; set; }
    }
}