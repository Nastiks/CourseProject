using CourseProject_DataAccess.Repository.IRepository;
using CourseProject_Models;
using CourseProject_Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject_DataAccess.Repository
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        private readonly ApplicationDbContext _db;
        public LikeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }       
    }
}
