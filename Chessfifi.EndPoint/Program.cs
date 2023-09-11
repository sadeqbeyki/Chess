using Chessfifi.EndPoint.MailSender;
using Chessfifi.Infrastructure;
using Chessfifi.Infrastructure.UnitOfWork;
using Chessfifi.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Chessfifi.Domain.ChessAgg;
using Chessfifi.Domain.UserAgg;
using Chessfifi.Infrastructure.Repositories;
using Chessfifi.Services.Service;
using NLog.Extensions.Logging;
using Chessfifi.EndPoint.Middlewares;
using Chessfifi.EndPoint.Hubs;
using Chessfifi.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

//builder.builder.Services.AddScoped<IGameRepository, GameRepository>();
//builder.builder.Services.AddDbContext<ApplicationDbContext>(c =>
//    c.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//====================================================================================================================
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var fckSpam = true;
var fckGrid = true;
if (fckSpam && !fckGrid)
{
    builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();
}
else
{
    builder.Services.AddTransient<IEmailSender, MyEmailSender>();
}

builder.Services.Configure<MailSenderConfig>(builder.Configuration.GetSection("MailSender"));
builder.Services.Configure<SendGridConfig>(builder.Configuration.GetSection("SendGrid"));

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Lockout settings.
    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    //options.Lockout.MaxFailedAccessAttempts = 5;
    //options.Lockout.AllowedForNewUsers = true;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
});

// todo 
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();

builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IGameManager, GameManager>();
builder.Services.AddSingleton<PieceTypesDto, PieceTypesDto>();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
    loggingBuilder.AddNLog(builder.Configuration);
});

var app = builder.Build();
// Configure the HTTP request pipeline.
//====================================================================================================================
//app.UseMigrationsEndPoint();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
    endpoints.MapHub<ChatHub>("/chatHub");
});

//app.MapHub<ChatHub>("/chatHub");
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
