namespace PostDatabase.Models.Requests
{
    public class MovieRequest
    {
        public string Id { get { return Guid.NewGuid().ToString(); } }
        public required string Title { get; set; }
        public int Year { get; set; }
        public required string Director { get; set; }
        public required string Video { get; set; }
        public required string Thumb { get; set; }
    }
}
