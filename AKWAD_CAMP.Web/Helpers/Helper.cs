using AKWAD_CAMP.Core.Interfaces;
using AKWAD_CAMP.EF;

namespace AKWAD_CAMP.Web.Helpers
{
    public static class Helper
    {
        private static  SalesContext _salesContext = new();
        private static  IUnitOfWork _context = new UnitOfWork(_salesContext) ;
        public static double GetPrice(int id)
        {
            var temp = _context.Templates.Get(criteria: x => !x.Deleted && x.TemplateId == id, includes: new[] { "Sub", "TemOfTempsSub", "TemplateItems" }).FirstOrDefault();
            double price = 0;
            if (temp == null) return price;
            var itemNumber_Price = _context.ItemsDetails.Get(criteria: x => x.Active).Select(e => new { e.ItemNumber, e.ItemPrice });
            if (!temp.TempOfTempInd)
            {
                var active_notDeleted_items = _context.ItemsDetails.Get(criteria: it => temp.TemplateItems.Select(i => i.ItemNumber).Contains(it.ItemNumber) && it.Active && !it.Deleted).Select(i => i.ItemNumber);
                if (temp.TemplateItems is null)
                    return price;
                var quantity = temp.TemplateItems.Where(x => active_notDeleted_items.Contains(x.ItemNumber)).Select(i => new { i.ItemNumber, i.Quantity });
                foreach (var item in quantity)
                {
                    var t = itemNumber_Price.FirstOrDefault(x => x.ItemNumber == item.ItemNumber).ItemPrice ?? 0;
                    price += (t * item.Quantity ?? 0);
                }
                price = (price) - (price * (double)(temp.Damage / 100)) + (price * (double)(temp.Profit / 100));
            }
            else
            {

                foreach (var template in temp.Sub)
                {
                    var Quantity = temp.TemOfTempsSup.Where(x => x.SubId == template.TemplateId).FirstOrDefault().Quantity;
                    if (template.TempOfTempInd)
                    {
                        price += GetPrice(template.TemplateId) * Quantity;
                    }
                    else
                    {

                        var templ = _context.Templates.Get(x => x.TemplateId == template.TemplateId, includes: new[] { "TemplateItems" }).FirstOrDefault();
                        var active_notDeleted_items = _context.ItemsDetails.Get(criteria: it => template.TemplateItems.Select(i => i.ItemNumber).Contains(it.ItemNumber) && it.Active && !it.Deleted).Select(i => i.ItemNumber);
                        var quantity = template.TemplateItems.Where(x => active_notDeleted_items.Contains(x.ItemNumber)).Select(i => new { i.ItemNumber, i.Quantity });
                        double tempPrice = 0;
                        foreach (var item in quantity)
                        {
                            var t = itemNumber_Price.FirstOrDefault(x => x.ItemNumber == item.ItemNumber).ItemPrice ?? 0;
                            tempPrice += (t * item.Quantity ?? 0);
                        }
                        price += ((tempPrice) - (tempPrice * (double)(template.Damage / 100)) + (tempPrice * (double)(template.Profit / 100))) * Quantity;
                    }

                }
            }
            return price;
        }


    }
}
