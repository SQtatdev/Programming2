using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int TeamId { get; set; }
        public required Team Team { get; set; }
    }
}