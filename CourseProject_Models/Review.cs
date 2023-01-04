using NpgsqlTypes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CourseProject_Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string? Title { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string? Author { get; set; }

        [DisplayName("Title of the piece")]
        [System.ComponentModel.DataAnnotations.Required]
        public string? NameObject { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string? Tags { get; set; }
        [NotMapped]
        public IEnumerable<string>? Likes { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string? Description { get; set; }

        [DisplayName("Image")]
        public string? ImageUrl { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [Range(0, 10, ErrorMessage = "The rating can be from 0 to 10")]
        public int Rating { get; set; }

        [DisplayName("Review creation date")]
        [System.ComponentModel.DataAnnotations.Required]
        public DateTime? DatePublication { get; set; }

        [Display(Name = "Category Type")]
        [System.ComponentModel.DataAnnotations.Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
        public IEnumerable<Comment>? Comments { get; set; }        
    }
}
