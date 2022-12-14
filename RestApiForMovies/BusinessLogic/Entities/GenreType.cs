using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestApiForMovies.BusinessLogic.Entities
{
    public class GenreType
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Movie_Genre> MovieGenres { get; set; }
    }
}