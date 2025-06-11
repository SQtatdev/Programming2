using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
        public class Ranking
        {
            public int Id { get; set; }
            public string UserId { get; set; }
            public ApplicationUser User { get; set; }

            // Добавьте эти обязательные свойства
            public int TournamentID { get; set; }
            public int TotalPoints { get; set; }
        }
    }