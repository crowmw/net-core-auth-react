using System.ComponentModel.DataAnnotations;

namespace NetCoreAuthReact.Dtos
{
    public class CredentialDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
