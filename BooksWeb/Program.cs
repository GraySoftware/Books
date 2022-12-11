using Books.DataAccess;
using Books.DataAccess.Repository;
using Books.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// if we want to register anything in our dependency injector do it here

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    // this method looks inside the block specifically named "ConnectionStrings" in our json file
    // and looks for the key given in its arguments
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();
builder.Services.AddMvc();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    // pattern defines the default controller/action a request will take. 
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
