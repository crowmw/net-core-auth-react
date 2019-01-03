using System.ComponentModel.DataAnnotations;

namespace NetCoreAuthReact.Dtos
{
    public class RegisterCredentialDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PasswordConfirm { get; set; }
    }
}
