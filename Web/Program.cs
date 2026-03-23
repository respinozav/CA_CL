using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

//SignalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Services.AddHttpContextAccessor();
// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// 🔐 REPOS
builder.Services.AddScoped<LoginRepository>();
builder.Services.AddScoped<MenuRepository>();
builder.Services.AddScoped<RegisterRepository>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<CiudadesRepository>();
builder.Services.AddScoped<GenerosRepository>();
builder.Services.AddScoped<ChatRepository>();
// 🧠 SESSION (LA USAMOS EN LOGIN)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔐 SESSION ANTES DE AUTH
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// 👉 Ruta inicial al LOGIN
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Login}/{id?}");

// HUB CHAT
app.MapHub<ChatHub>("/chatHub");

app.Run();
