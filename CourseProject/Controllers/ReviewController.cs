using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using CourseProject_DataAccess.Repository.IRepository;
using CourseProject_Models;
using CourseProject_Models.ViewModels;
using CourseProject_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CodeFixes;

namespace CourseProject.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _revRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IApplicationUserRepository _userRepo;
        private readonly ITagRepository _tagRepo;
        public ReviewController(IReviewRepository revRepo, IWebHostEnvironment webHostEnvironment, IApplicationUserRepository userRepo, ITagRepository tagRepo)
        {
            _revRepo = revRepo;
            _webHostEnvironment = webHostEnvironment;
            _userRepo = userRepo;
            _tagRepo = tagRepo;
        }

        public IActionResult Index()
        {
            IEnumerable<Review> objList = _revRepo.GetAll(includeProperties: "Category");
            return View(objList);
        }

        public IActionResult Upsert(int? id, string? author)
        {
            ReviewVM reviewVM = new()
            {
                Review = new Review()
                {
                    Author = author
                },
                CategorySelectList = _revRepo.GetAllDropdownList(WC.CategoryName)
            };

            reviewVM.Review!.Author = author ?? _userRepo.FirstOrDefault(u => u.UserName == User.Identity!.Name).FullName;
            
            if (id == null)
            {
                return View(reviewVM);
            }
            else
            {
                reviewVM.Review = _revRepo.Find(id.GetValueOrDefault());
                if (reviewVM.Review == null)
                {
                    return NotFound();
                }

                return View(reviewVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ReviewVM reviewVM)
        {
            if (ModelState.IsValid)
            {
                FixTags(reviewVM);
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (reviewVM.Review!.Id == 0 && files.Count != 0)
                {
                    string upload = webRootPath + WC.ImagePath;
                    string extension = Path.GetExtension(files[0].FileName);
                    string fileName = Guid.NewGuid().ToString() + extension;

                    Account account = new(WC.CloudName, WC.ApiKey, WC.ApiSecret);
                    Cloudinary cloudinary = new(account);
                    MemoryStream memoryStream = new();
                    files[0].CopyTo(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(fileName, memoryStream)
                    };
                    var uploadResult = cloudinary.Upload(uploadParams);

                    reviewVM.Review.ImageUrl = uploadResult!.SecureUri!.AbsoluteUri;

                    _revRepo.Add(reviewVM.Review);
                    CheckTag(reviewVM);
                }
                else
                {
                    var objFromDb = _revRepo.FirstOrDefault(u => u.Id == reviewVM.Review.Id, isTracking: false);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string extension = Path.GetExtension(files[0].FileName);
                        string fileName = Guid.NewGuid().ToString() + extension;

                        Account account = new(WC.CloudName, WC.ApiKey, WC.ApiSecret);
                        Cloudinary cloudinary = new(account);
                        MemoryStream memoryStream = new();
                        files[0].CopyTo(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(fileName, memoryStream)
                        };
                        var uploadResult = cloudinary.Upload(uploadParams);

                        reviewVM.Review.ImageUrl = uploadResult!.SecureUri!.AbsoluteUri;
                    }
                    else
                    {
                        if (objFromDb != null)
                        {
                            reviewVM.Review.ImageUrl = objFromDb!.ImageUrl;
                        }                        
                    }
                    if (reviewVM.Review.ImageUrl == null)
                    {
                        reviewVM.Review.ImageUrl = @"https://res.cloudinary.com/drazkgyna/image/upload/v1673101626/Review_u8axur.jpg";
                    }
                    _revRepo.Update(reviewVM.Review);
                }
                TempData[WC.Success] = "Action completed successfully";
                _revRepo.Save();
                return RedirectToAction("Index");
            }
            reviewVM.CategorySelectList = _revRepo.GetAllDropdownList(WC.CategoryName);

            return View(reviewVM);
        }

        private void FixTags(ReviewVM reviewVM)
        {
            if (string.IsNullOrWhiteSpace(reviewVM.Review!.Tags))
                return;
            List<string> tags = new();
            var parts = reviewVM.Review!.Tags.Split(' ');
            foreach (var item in parts)
            {
                string tag = item.Trim().Replace("#", string.Empty)
                                        .Replace("\n", string.Empty)
                                        .Replace("\t", string.Empty)
                                        .Replace("\r", string.Empty);
                if (!string.IsNullOrWhiteSpace(tag))
                {
                    tags.Add($"#{tag}");
                }
            }
            reviewVM.Review.Tags = string.Join(" ", tags);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Review review = _revRepo.FirstOrDefault(u => u.Id == id, includeProperties: "Category");
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _revRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, obj!.ImageUrl!);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _revRepo.Remove(obj);
            _revRepo.Save();
            TempData[WC.Success] = "Action completed successfully";
            return RedirectToAction("Index");
        }

        public void AddTag(string tag, int id)
        {
            ReviewVM reviewVM = new()
            {
                Review = _revRepo.FirstOrDefault(r => r.Id == id)
            };
            reviewVM.Review.Tags += " " + tag;
        }

        public IActionResult Preview(int id)
        {
            return RedirectToAction("Details", "Home", new { id });
        }

        public void CheckTag(ReviewVM reviewVM)
        {
            var tagsReview = reviewVM.Review!.Tags!.Split(' ').Distinct();
            IEnumerable<Tag> tags = _tagRepo.GetAll();
            int count = !tags.Any() ? 0 : tags.Max(x => x.Id);            
            foreach (var item in tagsReview)
            {
                if (tags.Any(x => x.Name == item))
                {
                    continue;
                }
                if(string.IsNullOrWhiteSpace(item) || item.Trim().All(x => x == '#'))
                {
                    continue;
                }
                _tagRepo.Add(new Tag { Id = ++count, Name = item });
            }            
        }

        #region
        [HttpGet]
        public IActionResult GetReviewList()
        {
            if (User.IsInRole(WC.AdminRole))
            {
                return Json(new { data = _revRepo.GetAll() });
            }
            else
            {
                var user = _userRepo.FirstOrDefault(u => u.UserName == User.Identity!.Name);
                return Json(new { data = _revRepo.GetAll(x => x.Author == user.FullName) });
            }

        }
        #endregion

        #region
        [HttpGet]
        public IActionResult GetTagList()
        {
            return Json(_tagRepo.GetAll().Select(x => x.Name).Take(10));
        }
        #endregion
    }
}