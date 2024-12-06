using KooliProjekt.Data;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Core.Types;
using System.Security.Policy;

public static class SeedData
{

    public static void Generate(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        if (context.Matches.Any())
        {
            return;
        }

        var user = new IdentityUser
        {
            UserName = "newuser@example.com",
            Email = "newuser@example.com",
            NormalizedUserName = "NEWUSER@EXAMPLE.COM", // Optional but recommended for case-insensitivity
            NormalizedEmail = "NEWUSER@EXAMPLE.COM" // Optional but recommended
        };


        userManager.CreateAsync(user, "Password123!").Wait();

        var match1 = new Match();
        match1.GameStart = DateTime.Now;
        match1.FirstTeamScore = 0;
        match1.SecondTeamScore = 0;
        match1.winner = "Tie";

        var prediction1 = new Prediction();
        prediction1.PredictedScroteFirstTeam = 1;
        prediction1.PredictedScoreSecondTeam = 0;
        prediction1.punktid = 1;

        var ranking1 = new ranking();
        ranking1.TotalPoint = 1;

        var team1 = new Team();
        team1.TeamName = "wolfes";
        team1.TeamPlayers = "arnold";

        var tournament1 = new Tournament();
        tournament1.TournmentName = "lames";
        tournament1.TournamentStart = DateTime.Now;
        tournament1.TournamentEnd = DateTime.Now;
        tournament1.TournamentPart = 2;
        tournament1.TournamentInfo = "iligal";



        context.SaveChanges();
    }

    internal static void Generate(ApplicationDbContext context)
    {
        throw new NotImplementedException();
    }
}