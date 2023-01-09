using CourseProject_DataAccess.Repository.IRepository;
using CourseProject_Models;
using CourseProject_Models.ViewModels;
using CourseProject_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CourseProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReviewRepository _revRepo;
        private readonly ICategoryRepository _catRepo;
        private readonly ICommentRepository? _comRepo;
        private readonly ITagRepository? _tagRepo;
        private readonly ICompositionRepository? _composRepo;
        private readonly IApplicationUserRepository? _userRepo;
        private readonly ILikeRepository? _likeRepo;

        public HomeController(IReviewRepository revRepo,
                              ICategoryRepository catRepo,
                              ICommentRepository? comRepo,
                              ITagRepository? tagRepo,
                              ICompositionRepository? composRepo,
                              IApplicationUserRepository? userRepo,
                              ILikeRepository? likeRepo)
        {
            _revRepo = revRepo;
            _catRepo = catRepo;
            _comRepo = comRepo;
            _tagRepo = tagRepo;
            _composRepo = composRepo;
            _userRepo = userRepo;
            _likeRepo = likeRepo;
        }

        public IActionResult Index(bool sortByRating, bool sortByDate)
        {
            HomeVM homeVM = new()
            {
                Reviews = _revRepo.GetAll(includeProperties: "Category"),
                Categories = _catRepo.GetAll(),
                Tags = _tagRepo!.GetAll()
            };

            if (sortByRating)
            {
                homeVM.Reviews = homeVM.Reviews.OrderByDescending(x => x.Rating);
            }

            if (sortByDate)
            {
                homeVM.Reviews = homeVM.Reviews.OrderByDescending(x => x.DatePublication);
            }
            return View(homeVM);
        }


        public IActionResult Details(int id)
        {
            DetailsVM detailsVM = new()
            {
                Review = _revRepo.FirstOrDefault(r => r.Id == id, includeProperties: "Category"),
                Like = HasLike(id),
                Comment = _comRepo!.GetAll(c => c.ReviewId == id),
                Composition = _composRepo!.GetAll(c => c.ReviewId == id),
                Rating = HasRating(id),
                UserRating = GetRating(id),
                Likes = GetLikes()
            };
            return View(detailsVM);
        }

        [Authorize]
        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<Like> likes = new();
            if (HttpContext.Session.Get<IEnumerable<Like>>(WC.SessionLike) != null
                && HttpContext.Session.Get<IEnumerable<Like>>(WC.SessionLike)!.Any())
            {
                likes = HttpContext.Session.Get<List<Like>>(WC.SessionLike)!;
            }
            likes.Add(new Like { ReviewId = id });
            HttpContext.Session.Set(WC.SessionLike, likes);
            TempData[WC.Success] = "The review has been added to the list of likes";
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult DeleteLike(int id)
        {
            ApplicationUser user = _userRepo!.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            Like like = _likeRepo!.FirstOrDefault(l => (l.ReviewId == id) && (l.UserId == user.Id));
            _likeRepo!.Remove(like);
            _likeRepo!.Save();
            TempData[WC.Success] = "The review was removed from the liked list";
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SetRating(int rating, int id)
        {
            int count = 0;
            if (_composRepo!.GetAll().Any())
            {
                count = _composRepo!.GetAll().Max(x => x.Id);
            }
            string name = _revRepo.FirstOrDefault(x => x.Id == id).Title!;
            string userId = _userRepo!.FirstOrDefault(u => u.UserName == User.Identity!.Name).Id;
            Composition composition = new()
            {
                Id = ++count,
                Name = name,
                ReviewId = id,
                Rating = rating,
                UserId = userId
            };
            _composRepo!.Add(composition);
            _composRepo.Save();
            return RedirectToAction(nameof(Details), new { id });
        }

        public bool HasRating(int reviewId)
        {
            bool hasRating = false;
            if ((User.IsInRole(WC.AdminRole) || User.IsInRole(WC.UserRole)))
            {
                ApplicationUser user = _userRepo!.FirstOrDefault(u => u.UserName == User.Identity!.Name);
                IEnumerable<Composition> compositions = _composRepo!.GetAll(c => c.ReviewId == reviewId);
                foreach (var item in compositions)
                {
                    if (item.UserId == user.Id)
                    {
                        hasRating = true;
                        break;
                    }
                }
            }
            return hasRating;
        }

        public bool HasLike(int reviewId)
        {
            bool hasLike = false;
            if ((User.IsInRole(WC.AdminRole) || User.IsInRole(WC.UserRole)))
            {
                ApplicationUser user = _userRepo!.FirstOrDefault(u => u.UserName == User.Identity!.Name);
                IEnumerable<Like> likes = _likeRepo!.GetAll(c => c.ReviewId == reviewId);
                foreach (var like in likes)
                {
                    if (like.UserId == user.Id)
                    {
                        hasLike = true;
                        break;
                    }
                }
            }
            return hasLike;

        }

        public int GetRating(int reviewId)
        {
            int userRating = -1;
            if ((User.IsInRole(WC.AdminRole) || User.IsInRole(WC.UserRole)))
            {
                IEnumerable<Composition> compositions = _composRepo!.GetAll(c => c.ReviewId == reviewId);
                ApplicationUser user = _userRepo!.FirstOrDefault(u => u.UserName == User.Identity!.Name);
                foreach (var item in compositions)
                {
                    if (item.UserId == user.Id)
                    {
                        userRating = item.Rating;
                        break;
                    }
                }
            }
            return userRating;
        }

        public IActionResult SetLike(int id)
        {
            int count = 0;
            if (_likeRepo!.GetAll().Any())
            {
                count = _likeRepo!.GetAll().Max(x => x.Id);
            }
            if (User.Identity!.Name != null)
            {
                ApplicationUser user = _userRepo!.FirstOrDefault(u => u.UserName == User.Identity!.Name);
                Like like = new()
                {
                    Id = ++count,
                    ReviewId = id,
                    UserId = user.Id
                };
                _likeRepo.Add(like);
                _likeRepo.Save();
            }
            return RedirectToAction(nameof(Details), new { id });
        }

        public Dictionary<string, int> GetLikes()
        {
            Dictionary<string, int> likes = new();
            IEnumerable<ApplicationUser> users = _userRepo!.GetAll();

            foreach (var user in users)
            {
                int count = 0;
                if (_comRepo!.FirstOrDefault(c => c.Author == user.FullName) != null)
                {
                    string name = _comRepo!.FirstOrDefault(c => c.Author == user.FullName).Author;
                    string id = _userRepo.FirstOrDefault(u => u.FullName == name).Id;
                    if (_likeRepo!.GetAll(l => l.UserId == id) != null)
                    {
                        count = _likeRepo!.GetAll(l => l.UserId == id).Count();
                    }
                }
                likes.Add(user.FullName!, count);
            }
            return likes;
        }

        [HttpGet]
        public IActionResult GetComments(int id)
        {
            var allLikes = GetLikes();
            var comments = _comRepo!.GetAll(u => u.ReviewId == id)
                .Select(x =>
                {
                    string author = x.Author;
                    string text = x.Text;
                    int likes = allLikes[author];
                    return new { author, text, likes };
                });

            return Json(new { data = comments });
        }
    }
}