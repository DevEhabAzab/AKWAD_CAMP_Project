using AKWAD_CAMP.Core.Interfaces;
using AKWAD_CAMP.Web.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.ComponentModel;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;
using OfficeOpenXml.Style;
using AKWAD_CAMP.Core.Contants;
using AKWAD_CAMP.Web.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using AKWAD_CAMP.Web.Services;
using Microsoft.AspNetCore.Authorization;
using AKWAD_CAMP.Web.Services.Repos;

namespace AKWAD_CAMP.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IDataExportation _iExport;
      
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IStringLocalizer<HomeController> localizer, IDataExportation iExport)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _iExport = iExport;
        }

        public IActionResult Index()
        {
            ViewBag.NewTemplate = _localizer["New Template"];
            ViewBag.More = _localizer["More"];
            ViewBag.Items = _localizer["Items"];
            ViewBag.Statuses = _localizer["Statuses"];
            ViewBag.Units = _localizer["Units"];
            return View();
        }



        public IActionResult ChartsData()
        {/*

            var top10expensive = _unitOfWork.ItemsDetails
                .Get(criteria: x => x.ItemPrice != null, orderBy: x => x.ItemPrice, orderDirection: OrderDirection.DESC, take: 10)
                .Select(x => new { item = x.ItemName,price = x.ItemPrice });


            var top10inttemp = _unitOfWork.TemplateItems.GetAll().GroupBy(x => x.ItemNumber)
                .Select(x => new
                {
                    item =_unitOfWork.ItemsDetails.Get(criteria: i=>i.ItemNumber == x.Key && i.Active && !i.Deleted).FirstOrDefault()?.ItemName??$"i{x.Key}" ,
                    count = x.Sum(x => x.Quantity)
                }).OrderByDescending(x => x.count).Take(10);
            var data = new
            {
                top10Expensive = top10expensive,
                top10InTemp = top10inttemp,
               
            };*/
            return Ok(/*data*/);
        }

        public int GetProductCount()
        {
            return 0;// _unitOfWork.ProductsDetails.Count();
        }
        public int GetUnitsCount()
        {
            return 67;//_unitOfWork.Units.Count();
        }
        public int GetTemplatesCount()
        {
            return 44;// _unitOfWork.Templates.Count();
        }
        public int GetItemsCount()
        {
            return 3;// _unitOfWork.ItemsDetails.Count();
        }




       /* public IActionResult DownloadExcelEPPlus(int id)
        {

            var superType = _unitOfWork.Templates.Find(id).TempOfTempInd;

            string excelName = $"Template-{id}-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            if (!superType)
            {
                var d = _iExport.Export(id);
                return File(d, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

            }
            else
            {
                var d = _iExport.ExportSuperTemp(id);
                return File(d, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            
            
          


        }
*/
     

        class ItemsViewModel
        {
            [Display(Name = "ItemNumber")]
            public string ItemNumber { get; set; }
            [Display(Name = "ItemPrice")]
            public double? ItemPrice { get; set; }
            [Display(Name = "ItemName")]
            public string ItemName { get; set; }
            [Display(Name = "Quantity")]
            public double ItemQuantity { get; set; }
            [Display(Name = "QuantityPrice")]
            public double? QuantityPrice { get; set; }
            [Display(Name = "ItemCategory")]
            public int? ItemCategory { get; set; }
            [Display(Name = "ItemUnit")]
            public string ItemUnit { get; set; }

        }
     


        public class UserInfo
        {
            public string UserName { get; set; }
            public int Age { get; set; }
        }

        [HttpPost]
        public IActionResult SetLanguage(string Cult , string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(Cult)),
                new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(1)
                }


                );
            return LocalRedirect(returnUrl);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}