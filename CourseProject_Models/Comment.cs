using System.ComponentModel.DataAnnotations;

namespace CourseProject_Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        public int ReviewId { get; set; }
    }
}
