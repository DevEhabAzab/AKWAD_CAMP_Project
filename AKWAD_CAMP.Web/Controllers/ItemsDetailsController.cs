#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AKWAD_CAMP.Core.Models;
using AKWAD_CAMP.EF;
using AKWAD_CAMP.Core.Interfaces;
using Newtonsoft.Json;
using AKWAD_CAMP.Core.Contants;
using AKWAD_CAMP.Web.ViewModels;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace AKWAD_CAMP.Web.Controllers
{

    public class ItemsDetailsController : Controller
    {
        private readonly IUnitOfWork _context;

        public ItemsDetailsController(IUnitOfWork context)
        {
            _context = context;
        }
        private string GetCategory(int? itemid,int? catid=null)
        
        {
            ItemsDetail item ;
            if (itemid is not null)
                item = _context.ItemsDetails.Get(criteria: e => e.ItemId == itemid, includes: new[] { "Category" }).FirstOrDefault();
            else
                item = null;
            int? cat = null;
            string? catname = "";
            if (item == null && catid == null)
                return "";
            else
            {
                var id = item is null ? catid : item.CategoryId;
                var temp = _context.ItemsCategories.Find(id);
                cat = temp?.ParentId;
                catname = temp?.CategoryName;
            }
            
            return   GetCategory(null, cat)+ "=>"+ catname;
        }

       

        // GET: ItemsDetails
        public IActionResult Index()
        {
            var salesContext = _context.ItemsDetails.Get(criteria: i => i.Deleted == false, includes: new[] { "Category", "Status", "Unit" }).Select(x => new ListItemViewModel
            {
                ItemId = x.ItemId,
                ItemNumber = x.ItemNumber,
                ItemDesc = x.ItemDesc,
                ItemName = x.ItemName,
                Category = x.Category?.CategoryName ?? "HAS NO CATEGORY",
                Unit = x.Unit?.UnitShortName ?? "HAS NO UNIT",
                Status = x.Status?.StatusName ?? "HAS NO STATUS",
                ItemPrice = x.ItemPrice ?? 0,
                Active = x.Active
            });


            return View(salesContext);
        }

        // GET: ItemsDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemsDetail =  _context.ItemsDetails
                .Get(includes: new[] { "Category", "Status", "Unit" })
                .FirstOrDefault(m => m.ItemId == id);
            if (itemsDetail == null)
            {
                return NotFound();
            }

            return View(itemsDetail);
        }

        // GET: ItemsDetails/Create
        public IActionResult Create()
        {

            ViewData["CategoryId"] = new SelectList(_context.ItemsCategories.Get(criteria:c=>c.Deleted==false && c.Active == true), "CategoryId", "CategoryName");
            ViewData["StatusId"] = new SelectList(_context.Statuses.Get(criteria: c => c.Deleted == false && c.Active == true), "StatusId", "StatusName");
            ViewData["SubCategoryId"] = new SelectList(_context.ItemsCategories.Get(criteria: c => c.Deleted == false && c.Active == true), "CategoryId", "CategoryName");
            ViewData["UnitId"] = new SelectList(_context.Units.Get(criteria: c => c.Deleted == false && c.Active == true), "UnitId", "UnitShortName");
            return View();
        }

        // POST: ItemsDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,ItemName,ItemDesc,UnitId,CategoryId,SubCategoryId,ItemPrice,ItemExpDate,LastUpdateUser,LastUpdateDate,StatusId,BeginDate,EndDate,Active,Deleted")] ItemsDetail itemsDetail)
        {
            if (ModelState.IsValid)
            {
                var LastItem = _context.ItemsDetails
                    .Get(criteria: x => x.ItemName == itemsDetail.ItemName
                    && x.Active && x.CategoryId == itemsDetail.CategoryId
                    && !x.Deleted).FirstOrDefault();
                if (LastItem != null)
                    itemsDetail.ItemNumber = LastItem.ItemNumber;
                else
                {
                    var nextnumber = await _context.NumberSeq(SeqName.ItemNumber);
                    itemsDetail.ItemNumber = "I_" + nextnumber.ToString();
                }
                itemsDetail.LastUpdateUser = User.Identity.Name;
                _context.ItemsDetails.Add(itemsDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.ItemsCategories.Get(criteria: c => c.Deleted == false && c.Active == true), "CategoryId", "CategoryName", itemsDetail.CategoryId);
            ViewData["StatusId"] = new SelectList(_context.Statuses.Get(criteria: c => c.Deleted == false && c.Active == true), "StatusId", "StatusName", itemsDetail.StatusId);
            ViewData["SubCategoryId"] = new SelectList(_context.ItemsCategories.Get(criteria: c => c.Deleted == false && c.Active == true), "CategoryId", "CategoryName", itemsDetail.SubCategoryId);
            ViewData["UnitId"] = new SelectList(_context.Units.Get(criteria: c => c.Deleted == false && c.Active == true), "UnitId", "UnitShortName", itemsDetail.UnitId);
            return View(itemsDetail);
        }

        // GET: ItemsDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemsDetail = await _context.ItemsDetails.FindAsync(id);
            if (itemsDetail == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.ItemsCategories.Get(criteria: c => c.Deleted == false && c.Active == true), "CategoryId", "CategoryName", itemsDetail.CategoryId);
            ViewData["StatusId"] = new SelectList(_context.Statuses.Get(criteria: c => c.Deleted == false && c.Active == true), "StatusId", "StatusName", itemsDetail.StatusId);
            ViewData["UnitId"] = new SelectList(_context.Units.Get(criteria: c => c.Deleted == false && c.Active == true), "UnitId", "UnitShortName", itemsDetail.UnitId); 
            return View(itemsDetail);
        }

        // POST: ItemsDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,ItemNumber,ItemName,ItemDesc,UnitId,CategoryId,SubCategoryId,ItemPrice,ItemExpDate,LastUpdateUser,LastUpdateDate,StatusId,BeginDate,EndDate,Active,Deleted")] ItemsDetail itemsDetail)
        {
            if (id != itemsDetail.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    itemsDetail.LastUpdateUser = User.Identity.Name;
                    _context.ItemsDetails.Update(itemsDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemsDetailExists(itemsDetail.ItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.ItemsCategories.Get(criteria: c => c.Deleted == false && c.Active == true), "CategoryId", "CategoryName", itemsDetail.CategoryId);
            ViewData["StatusId"] = new SelectList(_context.Statuses.Get(criteria: c => c.Deleted == false && c.Active == true), "StatusId", "StatusName", itemsDetail.StatusId);
            ViewData["SubCategoryId"] = new SelectList(_context.ItemsCategories.Get(criteria: c => c.Deleted == false && c.Active == true), "CategoryId", "CategoryName", itemsDetail.SubCategoryId);
            ViewData["UnitId"] = new SelectList(_context.Units.Get(criteria: c => c.Deleted == false && c.Active == true), "UnitId", "UnitShortName", itemsDetail.UnitId);
            return View(itemsDetail);
        }

        // GET: ItemsDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemsDetail =  _context.ItemsDetails
               .Get(includes: new string[] { "Category", "Status", "SubCategory", "Unit" })
                .FirstOrDefault(m => m.ItemId == id);
            if (itemsDetail == null)
            {
                return NotFound();
            }

            return View(itemsDetail);
        }

        // POST: ItemsDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemsDetail = await _context.ItemsDetails.FindAsync(id);
            itemsDetail.LastUpdateUser = User.Identity.Name;
            _context.ItemsDetails.BusinessDelete(itemsDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemsDetailExists(int id)
        {
            return _context.ItemsDetails.Any(e => e.ItemId == id);
        }
    }
}
