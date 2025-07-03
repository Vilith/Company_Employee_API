using System.ComponentModel.DataAnnotations;

namespace Companies.Shared.DTOs
{
    public record UserForAuthDTO([Required] string UserName, [Required] string PassWord);
}
