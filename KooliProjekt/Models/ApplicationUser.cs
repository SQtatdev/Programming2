// Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class ApplicationUser : IdentityUser
    {
        // Дополнительные свойства
        public string? CustomField { get; set; }
    }
}