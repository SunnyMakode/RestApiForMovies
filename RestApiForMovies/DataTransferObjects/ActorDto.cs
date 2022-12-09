using System.Text.Json.Serialization;

namespace RestApiForMovies.DataTransferObjects
{
    public class ActorDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}