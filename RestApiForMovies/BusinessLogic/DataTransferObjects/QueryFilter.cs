using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace RestApiForMovies.BusinessLogic.DataTransferObjects
{
    public class QueryFilter
    {
        public string MovieName { get; set; }
        public DirectorDto Director { get; set; }
        public int AgeLimit { get; set; }
        public int Rating { get; set; }
    }
}
