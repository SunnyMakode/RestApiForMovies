using System.Text.Json.Serialization;

namespace RestApiForMovies.BusinessLogic.Entities
{
    public class Actor
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]
        public ICollection<Movie_Actor> MovieActors { get; set; }
    }
}