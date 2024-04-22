using AKWAD_CAMP.Web.MiddleWares;

namespace AKWAD_CAMP.Web.MiddleWareEtentions
{
    public static class RequestCultureExtentions
    {
        public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCultureMiddleWare>();
        }
    }
}
