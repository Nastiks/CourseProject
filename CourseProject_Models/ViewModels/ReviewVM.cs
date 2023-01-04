using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseProject_Models.ViewModels
{
    public class ReviewVM
    {
        public Review? Review { get; set; }
        public IEnumerable<SelectListItem>? CategorySelectList { get; set; }
        public IEnumerable<Tag>? Tags { get; set; }
    }
}
