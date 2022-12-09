using System.Text.Json.Serialization;

namespace RestApiForMovies.Entities
{
    public class Movie_Director
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public int MovieId { get; set; }
        [JsonIgnore]
        public Movie Movie { get; set; }
        [JsonIgnore]
        public int DirectorId { get; set; }
        public Director Director { get; set; }
    }
}