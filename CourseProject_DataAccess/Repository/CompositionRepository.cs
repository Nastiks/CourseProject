﻿using CourseProject_DataAccess.Repository.IRepository;
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
    public class CompositionRepository : Repository<Composition>, ICompositionRepository
    {
        private readonly ApplicationDbContext _db;
        public CompositionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }       
    }
}
