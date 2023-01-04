using CourseProject_DataAccess.Repository.IRepository;
using CourseProject_Models;
using CourseProject_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentRepository? _comRepo;
        private readonly IReviewRepository _revRepo;
        private readonly IApplicationUserRepository _userRepo;
        public CommentController(ICommentRepository? comRepo, IReviewRepository revRepo, IApplicationUserRepository userRepo)
        {
            _comRepo = comRepo;
            _revRepo = revRepo;
            _userRepo = userRepo;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string text, int id)
        {
            Comment comment = new Comment();
            comment.Text = text;
            comment.ReviewId = id;
            comment.Author = _userRepo.FirstOrDefault(u => u.UserName == User.Identity!.Name).FullName!;
            comment.Date = DateTime.Now;
            _comRepo!.Add(comment);
            _comRepo.Save();
            TempData[WC.Success] = "Comment created successfully";
            return RedirectToAction("Details", "Home", new {id});
        }
    }
}
