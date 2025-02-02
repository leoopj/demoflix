namespace GetAllMovies.Models.Responses
{
    public class MovieResponse
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public int Year { get; set; }
        public string? Director { get; set; }
        public string? Video { get; set; }
        public string? Thumb { get; set; }
    }
}
