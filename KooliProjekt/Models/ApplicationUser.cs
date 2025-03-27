// Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;

namespace KooliProjekt.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Дополнительные свойства
        public string? CustomField { get; set; }
    }
}