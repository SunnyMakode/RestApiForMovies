using RestApiForMovies.Entities;
using System.Text.Json.Serialization;

namespace RestApiForMovies.DataTransferObjects
{
    public class MovieDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public List<string> Genres { get; set; } 
        public byte AgeLimit { get; set; }
        public byte Rating { get; set; }
        public List<ActorDto> Actors { get; set; }
        public DirectorDto Director { get; set; }
        public string Synopsis { get; set; }
    }
}
