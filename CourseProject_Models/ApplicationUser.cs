using Microsoft.AspNetCore.Identity;

namespace CourseProject_Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
