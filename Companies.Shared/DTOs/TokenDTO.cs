using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Shared.DTOs
{
    public record TokenDTO(string AccessToken, string RefreshToken);

}
