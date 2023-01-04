using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject_Models.ViewModels
{
    public class UserVM
    {
        public ApplicationUser? ApplicationUser { get; set; }
        public IEnumerable<Review>? Review { get; set; }
    }
}
