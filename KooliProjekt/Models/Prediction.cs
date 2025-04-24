using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class Prediction
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int UserId { get; set; }
        public int PredictedScoreFirstTeam { get; set; }
        public int PredictedScoreSecondTeam { get; set; }
        public int Punktid { get; set; }

        public string PredictedResult
        {
            get { return $"{PredictedScoreFirstTeam},{PredictedScoreSecondTeam}"; }
        }
    }
}