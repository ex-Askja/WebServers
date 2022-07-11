using Microsoft.EntityFrameworkCore;
using WebServers.Domain;
using WebServers.Domain.Data;
using WebServers.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Bind("Project", new Config());

builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddDbContext<AppDbContext>(context => {
    context.UseSqlServer(Config.ConnectionString, config => {
        config.CommandTimeout(600);
        config.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    });
});
builder.Services.AddScoped<IServerList, ServerList>();

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
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
