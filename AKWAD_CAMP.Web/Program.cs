using AKWAD_CAMP.Core.Interfaces;
using AKWAD_CAMP.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AKWAD_CAMP.Web.Areas.Identity.Data;
using Microsoft.Extensions.Localization;
using AKWAD_CAMP.Web.Localizer;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using AKWAD_CAMP.Web.MiddleWareEtentions;
using AKWAD_CAMP.Web.Services.Interfaces;
using AKWAD_CAMP.Web.Services.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Sales");;

builder.Services.AddDbContext<authContext>(options =>
    options.UseSqlServer(connectionString));;

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<authContext>();;

builder.Services.AddDbContext<SalesContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddLocalization();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.AddTransient<IDataExportation, DataExportation>();


ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;
// Add services to the container.

builder.Services.AddMvc(o =>
{
    o.EnableEndpointRouting = false;

    var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    o.Filters.Add(new AuthorizeFilter(policy));
    o.EnableEndpointRouting = false;

})
    //.AddJsonOptions(x => x. = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
    .AddNewtonsoftJson(x =>
    {
        x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;




    })
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(opt => 
    
        opt.DataAnnotationLocalizerProvider = (type , fact) => fact.Create(typeof(JsonStringLocalizerFactory))

    );



builder.Services.Configure<RequestLocalizationOptions>(opt =>
{
    var supportedCult = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("ar-EG")

    };
    //opt.DefaultRequestCulture = new RequestCulture(supportedCult[0], supportedCult[0]);
    opt.SupportedCultures = supportedCult;
    opt.SupportedUICultures = supportedCult;
});

builder.Services.AddRazorPages().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddDbContext<SalesContext>(opt=> {
    opt.UseSqlServer(configuration.GetConnectionString("Sales"));
});
builder.Services.AddKendo();
builder.Services.AddTransient(typeof(IUnitOfWork),typeof(UnitOfWork));
//builder.Services.AddIdentity<IdentityUser, IdentityRole>(option =>
//{
//    option.Password.RequiredLength = 4;
//    option.Password.RequiredUniqueChars = 2;
//}).AddEntityFrameworkStores<SalesContext>();





var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SalesContext>();
    var authdb = scope.ServiceProvider.GetRequiredService<authContext>();
    db.Database.Migrate();
    authdb.Database.Migrate();
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
var cults = new[]
{
    "en-US",
    "ar-EG"
};
var requestLoclizerOptions = new RequestLocalizationOptions()//.SetDefaultCulture(cults[0])
    .AddSupportedCultures( cults)
    .AddSupportedUICultures(cults);
app.UseRequestLocalization(requestLoclizerOptions);

app.UseAuthentication();;

app.UseAuthorization();
app.UseRequestCulture();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
