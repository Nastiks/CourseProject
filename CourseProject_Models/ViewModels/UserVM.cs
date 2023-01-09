namespace CourseProject_Models.ViewModels
{
    public class UserVM
    {
        public ApplicationUser? ApplicationUser { get; set; }
        public IEnumerable<Review>? Review { get; set; }
    }
}
