// Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; } // Пример дополнительного поля
}