using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    [GenerateSerializer, Immutable]
    public record RegisterMessageDTO (
        [property: Id(0)] string SenderId,
        [property: Id(1)] string ChatRoomId,
        [property: Id(2)] string Text );

    [GenerateSerializer, Immutable]
    public record DisplayMessageDto(
        [property: Id(0)] string SenderId,
        [property: Id(1)] string Text );
}
