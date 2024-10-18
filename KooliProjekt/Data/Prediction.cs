namespace KooliProjekt.Data
{
    public class Prediction
    {
        public int Id { get; set; }
        public int MacthId { get; set; }
        public int UserId { get; set; }
        public int PredictedScroteFirstTeam {  get; set; }
        public int PredictedScoreSecondTeam { get; set; }
        public int punktid { get; set; }
    }
}
