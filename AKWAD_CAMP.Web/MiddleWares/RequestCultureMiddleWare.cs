using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace AKWAD_CAMP.Web.MiddleWares
{
    public class RequestCultureMiddleWare 
    {
        private readonly RequestDelegate _next;

        public RequestCultureMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var CurrentLang = context.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            var browserLang = context.Request.Headers["Accept-Language"].ToString()[..2];
            string defaultCult = string.Empty;
            if (string.IsNullOrEmpty(CurrentLang))
            {
                if (browserLang == "ar")
                    defaultCult = "ar-EG";
                else
                    defaultCult = "en-US";

                var requestCult = new RequestCulture(defaultCult, defaultCult);
                context.Features.Set<IRequestCultureFeature>(new RequestCultureFeature(requestCult, null));

                CultureInfo.CurrentCulture = new CultureInfo(defaultCult);
                CultureInfo.CurrentUICulture = new CultureInfo(defaultCult);
            }
            
            
            await _next(context);
        } 
    }
}
