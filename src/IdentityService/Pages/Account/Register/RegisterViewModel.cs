using System.ComponentModel.DataAnnotations;

namespace IdentityService.Pages.Account.Register;

public class RegisterViewModel
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string ReturnUrl { get; set; } = string.Empty;
    public string? Button { get; set; }
}
