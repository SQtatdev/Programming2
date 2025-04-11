using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using ApplicationDbContext = KooliProjekt.Data.ApplicationDbContext;

namespace KooliProjekt
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Регистрация сервисов
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // 1. Регистрация контекста базы данных
            builder.Services.AddDbContext<Data.ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // 2. Регистрация Identity с кастомным пользователем
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // 3. Регистрация сервисов приложения
            builder.Services.AddScoped<IMatchService, MatchService>();
            builder.Services.AddScoped<IPredictionService, PredictionService>();
            builder.Services.AddScoped<IRankingServices, RankingServices>();
            builder.Services.AddScoped<ITeamService, TeamService>();
            builder.Services.AddScoped<ITournamentService, TournamentService>();


            // 4. Настройка контроллеров и Razor Pages
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Конфигурация middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            // Program.cs
            builder.Services.AddCors(options =>
                options.AddPolicy("AllowAll", policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader()));

            // 5. Заполнение базы начальными данными с обработкой ошибок
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    var context = services.GetRequiredService<Data.ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    await SeedData.GenerateAsync(context, userManager);
                } 
                catch (Exception ex)
                {
                    logger.LogError(ex, "Ошибка при заполнении базы данных");
                }
            }

            app.Run();
        }
    }
}