using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestApiForMovies.Entities
{
    public class Movie_Actor
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public int MovieId { get; set; }
        [JsonIgnore]
        public Movie Movie { get; set; }
        [JsonIgnore]
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}