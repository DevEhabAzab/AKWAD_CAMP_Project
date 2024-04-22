using AKWAD_CAMP.Core.Contants;
/*using AKWAD_CAMP.Core.Models;*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKWAD_CAMP.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
       
        /*IBaseRepo<ItemsCategory> ItemsCategories { get; }
        IBaseRepo<ItemsDetail> ItemsDetails { get; }
        IBaseRepo<ProductItem> ProductItems { get; }
        IBaseRepo<ProductsDetail> ProductsDetails { get; }
        IBaseRepo<Status> Statuses { get; }
        IBaseRepo<Template> Templates { get; }
        IBaseRepo<Unit> Units { get; }
        IBaseRepo<TemplateItems> TemplateItems { get; }
        IBaseRepo<TamplateOfTemplates> TamplateOfTemplates { get; }
*/
        int SaveChanges();
        Task<int> NumberSeq(SeqName seqName);
       
        Task<int> SaveChangesAsync();
    }
}
