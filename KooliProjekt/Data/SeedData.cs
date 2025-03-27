using Bogus;
using KooliProjekt.Models;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data
{
    public static class SeedData
    {
        public static async Task GenerateAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager) // Изменили IdentityUser → ApplicationUser
        {
            // Генерация пользователей
            var userFaker = new Faker<ApplicationUser>() // Исправлен тип
                .RuleFor(u => u.UserName, f => f.Internet.Email())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.EmailConfirmed, true);

            var users = userFaker.Generate(5);
            foreach (var user in users)
            {
                var result = await userManager.CreateAsync(user, "Password123!");
                if (!result.Succeeded)
                {
                    throw new Exception($"Ошибка создания пользователя: {string.Join(", ", result.Errors)}");
                }
            }

            // Генерация команд
            var teamFaker = new Faker<Team>()
                .RuleFor(t => t.TeamName, f => f.Company.CompanyName());

            var teams = teamFaker.Generate(10);
            await context.Teams.AddRangeAsync(teams);

            // Генерация турниров
            var tournamentFaker = new Faker<Tournament>()
                .RuleFor(t => t.Name, f => f.Lorem.Word());

            var tournaments = tournamentFaker.Generate(5);
            await context.Tournaments.AddRangeAsync(tournaments);

            await context.SaveChangesAsync(); // Сохраняем команды и турниры вместе

            // Генерация матчей
            var matchFaker = new Faker<Match>()
                .RuleFor(m => m.FirstTeamId, f => f.PickRandom(teams).Id)
                .RuleFor(m => m.SecondTeamId, f => f.PickRandom(teams).Id)
                .RuleFor(m => m.TournamentId, f => f.PickRandom(tournaments).Id)
                .RuleFor(m => m.Date, (f, m) => DateOnly.FromDateTime(f.Date.Future()));

            var matches = matchFaker.Generate(50);
            await context.Matches.AddRangeAsync(matches);

            await context.SaveChangesAsync();
        }
    }
}