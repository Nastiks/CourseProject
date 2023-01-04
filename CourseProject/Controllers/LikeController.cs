 using CourseProject_DataAccess;
using CourseProject_DataAccess.Repository.IRepository;
using CourseProject_Models;
using CourseProject_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CourseProject.Controllers
{
    [Authorize]
    public class LikeController : Controller
    {
        private readonly IReviewRepository _revRepo;
        private readonly ILikeRepository _likeRepo;
        private readonly IApplicationUserRepository _userRepo;
        public LikeController(IReviewRepository revRepo, ILikeRepository likeRepo, IApplicationUserRepository userRepo)
        {
            _revRepo = revRepo;
            _likeRepo = likeRepo;
            _userRepo = userRepo;
        }

        public IActionResult Index()
        {
            ApplicationUser user = _userRepo.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            IEnumerable<Like> likes = _likeRepo.GetAll(l => l.UserId == user.Id);
            List<int> reviewLike = new List<int>();
            foreach (var like in likes)
            {
                reviewLike.Add(like.ReviewId);
            }
            IEnumerable<Review> reviews = _revRepo.GetAll(r => reviewLike.Contains(r.Id)); 
            return View(reviews);
        }

        public IActionResult Delete(int id)
        {
            _likeRepo.Remove(_likeRepo.FirstOrDefault(l => l.ReviewId == id)!);
            _likeRepo.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
