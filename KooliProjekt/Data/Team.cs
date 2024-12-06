using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data
{
    public class Team
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public string TeamPlayers { get; set; }

    }
}
