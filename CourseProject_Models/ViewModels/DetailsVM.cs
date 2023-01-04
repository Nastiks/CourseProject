namespace CourseProject_Models.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            Review = new Review();
        }
        public Review? Review { get; set; }
        public IEnumerable<Comment>? Comment { get; set; }
        public IEnumerable<Composition>? Composition { get; set; }
        public bool Like { get; set; }
        public Dictionary<string, int> Likes { get; set; }
        public bool Rating { get; set; }
        public int UserRating { get; set; }
    }
}
