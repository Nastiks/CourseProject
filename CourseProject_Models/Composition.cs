using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
