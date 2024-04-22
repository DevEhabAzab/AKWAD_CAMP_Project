using AKWAD_CAMP.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AcceptVerbsAttribute = Microsoft.AspNetCore.Mvc.AcceptVerbsAttribute;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace AKWAD_CAMP.Web.Controllers
{
    public class ValidationController : Controller
    {
        private readonly IUnitOfWork _context;

        public ValidationController(IUnitOfWork context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
/*
        [AcceptVerbs("Get", "post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsStatusCodeValid(string StatusCode)
        {
            var status = _context.Statuses.Get(s => s.StatusCode == StatusCode);
            if (status == null)
                 return Json(true);
            return Json($"this Status Code ({StatusCode}) is in use");
        }
        [AcceptVerbs("Get", "post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsUnitNameValid(string UnitName)
        {
            var status = _context.Units.Get(s => s.UnitName == UnitName);
            if (status == null)
                return Json(true);
            return Json($"Unit with Name ({UnitName}) is in use");
        }
        [AcceptVerbs("Get", "post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsCategoryNumberValid(string CategoryNumber)
        {
            var status = _context.ItemsCategories.Get(s => s.CategoryNumber == CategoryNumber);
            if (status == null)
                return Json(true);
            return Json($"Category with Number ({CategoryNumber}) is in use");
        }*/
    }
}
