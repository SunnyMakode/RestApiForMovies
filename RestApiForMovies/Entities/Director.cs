using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestApiForMovies.Entities
{
    public class Director
    {
        [JsonIgnore]
        public int DirectorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]        
        public int MovieId { get; set; }
        [JsonIgnore]
        public Movie Movies { get; set; }
    }
}