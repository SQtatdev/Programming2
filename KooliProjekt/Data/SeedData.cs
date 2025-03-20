using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

public static class SeedData
{
    public static void Generate(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        // Если данные уже существуют, то выходим
        if (context.Matches.Any() || context.Users.Any() || context.Teams.Any() || context.Tournaments.Any())
        {
            return;
        }

        // Создание пользователей
        var user1 = new IdentityUser
        {
            UserName = "newuser1@example.com",
            Email = "newuser1@example.com",
            NormalizedUserName = "NEWUSER1@EXAMPLE.COM",
            NormalizedEmail = "NEWUSER1@EXAMPLE.COM"
        };
        userManager.CreateAsync(user1, "Password123!").Wait();

        var user2 = new IdentityUser
        {
            UserName = "newuser2@example.com",
            Email = "newuser2@example.com",
            NormalizedUserName = "NEWUSER2@EXAMPLE.COM",
            NormalizedEmail = "NEWUSER2@EXAMPLE.COM"
        };
        userManager.CreateAsync(user2, "Password123!").Wait();

        // Создание команд
        var team1 = new Team { TeamName = "Wolves", TeamPlayers = "Arnold, John" };
        var team2 = new Team { TeamName = "Lions", TeamPlayers = "Mike, Steve" };
        var team3 = new Team { TeamName = "Tigers", TeamPlayers = "Anna, Jane" };
        var team4 = new Team { TeamName = "Bears", TeamPlayers = "David, Chris" };
        var team5 = new Team { TeamName = "Eagles", TeamPlayers = "Kevin, Sam" };
        var team6 = new Team { TeamName = "Sharks", TeamPlayers = "Bob, Mark" };
        var team7 = new Team { TeamName = "Panthers", TeamPlayers = "Alex, Tom" };
        var team8 = new Team { TeamName = "Bulls", TeamPlayers = "James, David" };
        context.Teams.AddRange(team1, team2, team3, team4, team5, team6, team7, team8);

        // Создание турниров
        var tournament1 = new Tournament
        {
            TournmentName = "Summer Cup",
            TournamentStart = DateTime.Now.AddMonths(-1),
            TournamentEnd = DateTime.Now.AddMonths(1),
            TournamentPart = 1,
            TournamentInfo = "A tournament for all teams."
        };
        var tournament2 = new Tournament
        {
            TournmentName = "Winter League",
            TournamentStart = DateTime.Now.AddMonths(-2),
            TournamentEnd = DateTime.Now.AddMonths(2),
            TournamentPart = 2,
            TournamentInfo = "A winter league with various teams."
        };
        var tournament3 = new Tournament
        {
            TournmentName = "Autumn Tournament",
            TournamentStart = DateTime.Now.AddMonths(-3),
            TournamentEnd = DateTime.Now.AddMonths(3),
            TournamentPart = 1,
            TournamentInfo = "Tournament for the Autumn season."
        };
        var tournament4 = new Tournament
        {
            TournmentName = "Spring Tournament",
            TournamentStart = DateTime.Now.AddMonths(-4),
            TournamentEnd = DateTime.Now.AddMonths(4),
            TournamentPart = 2,
            TournamentInfo = "A tournament for the Spring season."
        };
        context.Tournaments.AddRange(tournament1, tournament2, tournament3, tournament4);

        // Создание матчей
        var match1 = new Match
        {
            GameStart = DateTime.Now.AddDays(5),
            FirstTeamScore = 0,
            SecondTeamScore = 0,
            FirstTeam = team1,
            SecondTeam = team2,
            Winner = "Tie"
        };
        var match2 = new Match
        {
            GameStart = DateTime.Now.AddDays(6),
            FirstTeamScore = 1,
            SecondTeamScore = 2,
            FirstTeam = team3,
            SecondTeam = team4,
            Winner = "Team 4"
        };
        var match3 = new Match
        {
            GameStart = DateTime.Now.AddDays(7),
            FirstTeamScore = 2,
            SecondTeamScore = 3,
            FirstTeam = team5,
            SecondTeam = team6,
            Winner = "Team 6"
        };
        var match4 = new Match
        {
            GameStart = DateTime.Now.AddDays(8),
            FirstTeamScore = 2,
            SecondTeamScore = 1,
            FirstTeam = team7,
            SecondTeam = team8,
            Winner = "Team 7"
        };
        context.Matches.AddRange(match1, match2, match3, match4);

        // Создание предсказаний
        var prediction1 = new Prediction
        {
            PredictedScroteFirstTeam = 1,
            PredictedScoreSecondTeam = 0,
            punktid = 1,
            Match = match1,
            UserId = user1.Id
        };
        var prediction2 = new Prediction
        {
            PredictedScroteFirstTeam = 2,
            PredictedScoreSecondTeam = 1,
            punktid = 2,
            Match = match2,
            UserId = user1.Id
        };
        var prediction3 = new Prediction
        {
            PredictedScroteFirstTeam = 2,
            PredictedScoreSecondTeam = 1,
            punktid = 3,
            Match = match3,
            UserId = user1.Id
        };
        var prediction4 = new Prediction
        {
            PredictedScroteFirstTeam = 2,
            PredictedScoreSecondTeam = 0,
            punktid = 1,
            Match = match4,
            UserId = user2.Id
        };
        context.Predictions.AddRange(prediction1, prediction2, prediction3, prediction4);

        // Создание рейтингов
        var ranking1 = new Ranking { TotalPoint = 10, UserId = user1.Id };
        var ranking2 = new Ranking { TotalPoint = 15, UserId = user1.Id };
        var ranking3 = new Ranking { TotalPoint = 20, UserId = user1.Id };
        var ranking4 = new Ranking { TotalPoint = 5, UserId = user2.Id };
        var ranking5 = new Ranking { TotalPoint = 8, UserId = user2.Id };
        var ranking6 = new Ranking { TotalPoint = 18, UserId = user2.Id };
        context.Rankings.AddRange(ranking1, ranking2, ranking3, ranking4, ranking5, ranking6);

        // Сохранение всех данных в базе данных
        context.SaveChanges();
    }

    internal static void Generate(ApplicationDbContext context)
    {
        // Для генерации данных без пользователей можно добавить такой же код, как в основном методе.
    }
}
