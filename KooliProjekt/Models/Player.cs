// Models/Player.cs
namespace KooliProjekt.Models
{
    public class Player
    {
        public int Id { get; set; }
        public required string PlayerName { get; set; }
        public int TeamId { get; set; }
        public required Team Team { get; set; }
    }
}