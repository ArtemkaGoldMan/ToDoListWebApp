using Microsoft.EntityFrameworkCore;
using ToDoWEB.Data;
using ToDoWEB.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ToDoContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ToDoAppDb;Trusted_Connection=True;MultipleActiveResultSets=true"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); 
}

app.UseHttpsRedirection(); 
app.UseStaticFiles();      

app.UseRouting();          

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Task}/{action=Index}/{id?}");

app.Run();
