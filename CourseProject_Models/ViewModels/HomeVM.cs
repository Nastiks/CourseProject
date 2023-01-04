namespace CourseProject_Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Review>? Reviews { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Tag>? Tags { get; set; }
    }
}
