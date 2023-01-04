using CourseProject_DataAccess.Repository.IRepository;
using CourseProject_Models;
using CourseProject_Models.ViewModels;
using CourseProject_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class UserController : Controller
    {
        private readonly IApplicationUserRepository? _userRepo;
        private readonly IReviewRepository? _revRepo;

        [BindProperty]
        public UserVM? UserVM { get; set; }

        public UserController(IApplicationUserRepository? userRepo, IReviewRepository? revRepo)
        {
            _userRepo = userRepo;
            _revRepo = revRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(string id)
        {
            UserVM = new UserVM()
            {
                ApplicationUser = _userRepo!.FirstOrDefault(u => u.Id == id)
            };
            UserVM.Review = _revRepo!.GetAll(u => u.Author == UserVM.ApplicationUser.FullName);
            
            return View(UserVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Details")]
        public IActionResult DetailsPost(string id)
        {
            UserVM = new UserVM()
            {
                ApplicationUser = _userRepo!.FirstOrDefault(u => u.Id == id)
            };
            Review review = new Review()
            {
                Author = UserVM.ApplicationUser.FullName
            };
            return RedirectToAction("Upsert", "Review", new { review.Author });
        }

        [HttpPost]
        public IActionResult Delete()
        {
            ApplicationUser applicationUser = _userRepo!.FirstOrDefault(u => u.Id == UserVM!.ApplicationUser!.Id);
            IEnumerable<Review> reviews = _revRepo!.GetAll(u => u.Author == UserVM!.ApplicationUser!.FullName);

            _revRepo.RemoveRange(reviews);
            _userRepo.Remove(applicationUser);
            _userRepo.Save();
            TempData[WC.Success] = "Action completed successfully";
            return RedirectToAction(nameof(Index));
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetUserList()
        {
            return Json(new {data = _userRepo.GetAll()});
        }
        #endregion
    }
}
