using CourseProject_DataAccess.Repository.IRepository;
using CourseProject_Models;
using CourseProject_Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing.Text;
using System.Linq;
using static Humanizer.On;

namespace CourseProject.Controllers
{
    public class SearchController : Controller
    {
        private readonly IReviewRepository _revRepo;
        private readonly ICommentRepository? _comRepo;

        public SearchController(IReviewRepository revRepo, ICommentRepository? comRepo)
        {
            _revRepo = revRepo;
            _comRepo = comRepo;
        }
        public IActionResult Index(string search)
        {
            if (search == null)
            {
                return RedirectToAction("Index", "Home");
            }
            search = search.ToLower();
            search = $"%{search}%";
            SearchVM searchVM = new()
            {
                Reviews = _revRepo.GetAll(r => EF.Functions.Like(r.Title!.ToLower(), search)
                                            || EF.Functions.Like(r.Description!.ToLower(), search)
                                            || EF.Functions.Like(r.NameObject!.ToLower(), search), includeProperties: "Category")
            };

            var found = SearchReviewByComment(search);
            searchVM.Reviews = searchVM.Reviews.Concat(found).DistinctBy(x => x.Id);
            return View(searchVM.Reviews);
        }

        private IEnumerable<Review> SearchReviewByComment(string query)
        {
            IEnumerable<Comment> comments = _comRepo!.GetAll(c => EF.Functions.Like(c.Text!.ToLower(), query));
            List<Review> reviews = new List<Review>();
            
            foreach (var comment in comments)
            {
                Review foundReview = _revRepo.FirstOrDefault(r => r.Id == comment.ReviewId, includeProperties: "Category");
                reviews.Add(foundReview);
            }
            return reviews;
        }
    }

    
}
