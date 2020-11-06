using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeBites.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

namespace CafeBites.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext context;

        public CategoryController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categoriesList = await context.Categories.ToListAsync();
            return View(categoriesList);
        }

        //public async Task<IActionResult> Create()
        //{
        //    if(ModelState.IsValid)
        //    {

        //    }
        //}
    }
}
