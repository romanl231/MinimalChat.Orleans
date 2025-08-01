using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public record RegisterDto(string Nickname, string Description, string Password);
    public record LoginDto(string Nickname, string Password);
    public record UserProfileDto(string Nickname, string Description);
}
