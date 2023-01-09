using System.ComponentModel.DataAnnotations;

namespace CourseProject_Models
{
    public class Composition
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int ReviewId { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public string? UserId { get; set; }
    }
}
