using Microsoft.AspNetCore.Mvc;

namespace AKWAD_CAMP.Web.Services.Interfaces
{
    public interface IDataExportation
    {
         MemoryStream ExportTemplate(int id);
        MemoryStream ExportProduct(int id);
        public MemoryStream ExportSuperTemp(int id);
        public MemoryStream Export(int id);

    }
}
