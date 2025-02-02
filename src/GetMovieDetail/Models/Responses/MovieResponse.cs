namespace GetMovieDetail.Models.Responses
{
    public class MovieResponse
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public int Year { get; set; }
        public string? Director { get; set; }
        public string? Video { get; set; }
        public string? Thumb { get; set; }

        public void CreateMovieResponse()
        {
            Id = "1";
            Title = "The Shawshank Redemption";
            Year = 1994;
            Director = "Frank Darabont";
            Video = "https://www.youtube.com/watch?v=6hB3S9bIaco";
            Thumb = "https://m.media-amazon.com/images/I/51S9gFmZGHL._AC_SY445_.jpg";
        }
    }
}
