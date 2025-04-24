// Models/Player.cs
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class Player
    {
        public int Id { get; set; }
        public required string PlayerName { get; set; }
        public int TeamId { get; set; }
        public required Team Team { get; set; }
    }
}