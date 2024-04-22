using AKWAD_CAMP.Core.Contants;
using AKWAD_CAMP.Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AKWAD_CAMP.EF
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly SalesContext _salesContext;

        public UnitOfWork(SalesContext salesContext)
        {
            _salesContext = salesContext;
           /* ItemsCategories = new BaseRepo<ItemsCategory>(salesContext);
            ItemsDetails = new BaseRepo<ItemsDetail>(salesContext);
            ProductItems = new BaseRepo<ProductItem>(salesContext);
            Statuses = new BaseRepo<Status>(salesContext);
            Templates = new BaseRepo<Template>(salesContext);
            Units = new BaseRepo<Unit>(salesContext);
            ProductsDetails = new BaseRepo<ProductsDetail>(salesContext);
            TemplateItems = new BaseRepo<TemplateItems>(salesContext);
            TamplateOfTemplates = new BaseRepo<TamplateOfTemplates>(salesContext);

*/
        }

       
/*
        public IBaseRepo<ItemsCategory> ItemsCategories { get; private set; }
        public IBaseRepo<TamplateOfTemplates> TamplateOfTemplates { get; private set; }

        public IBaseRepo<ItemsDetail> ItemsDetails { get; private set; }

        public IBaseRepo<ProductItem> ProductItems { get; private set; }

        public IBaseRepo<ProductsDetail> ProductsDetails { get; private set; }

        public IBaseRepo<Status> Statuses { get; private set; }

        public IBaseRepo<Template> Templates { get; private set; }

        public IBaseRepo<Unit> Units { get; private set; }

        public IBaseRepo<TemplateItems> TemplateItems { get; private set; }
*/
    public void Dispose()
        {
            _salesContext.Dispose();
        }

        public async Task<int> NumberSeq(SeqName seqName)
        {
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            string seq = seqName.GetType().GetMember(seqName.ToString()).First().GetCustomAttribute<DescriptionAttribute>().Description;

            await _salesContext.Database.ExecuteSqlRawAsync($"SELECT @result = (NEXT VALUE FOR [{seq}])", result);
            return (int)result.Value;
        }

       

        public int SaveChanges()
        {
            return _salesContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
           return await _salesContext.SaveChangesAsync();
        }
    }
}
