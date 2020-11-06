using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeBites.Data;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> Create()
        //{
        //    if(ModelState.IsValid)
        //    {

        //    }
        //}
    }
}
