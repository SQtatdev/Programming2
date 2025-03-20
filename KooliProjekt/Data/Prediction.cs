namespace KooliProjekt.Data
{
    public class Prediction
    {
        public int Id { get; set; }
        public int MatchId { get; set; }  // Связь с матчем
        public int UserId { get; set; }  // Связь с пользователем
        public int PredictedScoreFirstTeam { get; set; }
        public int PredictedScoreSecondTeam { get; set; }
        public DateTime PredictionDate { get; set; }

        // Навигационные свойства
        public virtual Match Match { get; set; }
        public virtual User User { get; set; }
    }
}
