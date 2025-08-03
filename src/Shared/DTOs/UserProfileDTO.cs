using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    [GenerateSerializer, Immutable]
    public record RegisterDto(
        [property: Id(0)] string Nickname,
        [property: Id(1)] string Description,
        [property: Id(2)] string Password);

    [GenerateSerializer, Immutable]
    public record LoginDto(
        [property: Id(0)] string Nickname, 
        [property: Id(1)] string Password);

    [GenerateSerializer, Immutable]
    public record UserProfileDto(
        [property: Id(0)]  string Nickname, 
        [property: Id(1)]  string Description);

    [GenerateSerializer, Immutable]
    public record ChangePasswordDto(
        [property: Id(0)] string Nickname, 
        [property: Id(1)] string CurrentPassword, 
        [property: Id(2)] string NewPassword);
}
